using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;

namespace Cuemon.Xml.Serialization.Converters
{
    /// <summary>
    /// Converts an <see cref="Exception"/> to XML.
    /// </summary>
    /// <seealso cref="XmlConverter" />
    public class ExceptionConverter : XmlConverter<Exception>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionConverter"/> class.
        /// </summary>
        /// <param name="includeStackTrace">A value that indicates if the stack of an exception is included in the converted result.</param>
        /// <param name="includeData">A value that indicates if the data of an exception is included in the converted result.</param>
        public ExceptionConverter(bool includeStackTrace = false, bool includeData = false)
        {
            IncludeStackTrace = includeStackTrace;
            IncludeData = includeData;
        }

        /// <summary>
        /// Gets a value indicating whether the data of an exception is included in the converted result.
        /// </summary>
        /// <value><c>true</c> if the data of an exception is included in the converted result; otherwise, <c>false</c>.</value>
        public bool IncludeData { get; }

        /// <summary>
        /// Gets a value indicating whether the stack of an exception is included in the converted result.
        /// </summary>
        /// <value><c>true</c> if the stack of an exception is included in the converted result; otherwise, <c>false</c>.</value>
        public bool IncludeStackTrace { get; }

        /// <summary>
        /// Writes the XML representation of the <paramref name="value" />.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
        public override void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null)
        {
            var exceptionType = value.GetType();
            writer.WriteStartElement(Decorator.Enclose(exceptionType.Name).SanitizeXmlElementName());
            if (exceptionType.Namespace != null) { writer.WriteAttributeString("namespace", exceptionType.Namespace); }
            WriteExceptionCore(writer, (Exception)value, IncludeStackTrace, IncludeData);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Reads the XML representation of the <paramref name="objectType" />.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> to read from.</param>
        /// <param name="objectType">The <seealso cref="T:System.Type" /> of the object.</param>
        /// <returns>An object of <paramref name="objectType" />.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object ReadXml(XmlReader reader, Type objectType)
        {
            var stack = new Stack<Dictionary<string, object>>();
            var properties = new List<PropertyInfo>();
            string exception = null;
            string lastException = null;
            var blueprints = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        exception = reader.Name.EndsWith("Exception") ? reader.Name : lastException;

                        if (blueprints.Count > 0 && blueprints.TryGetValue("Type", out var typeOfException))
                        {
                            if (!((Type)typeOfException).Name.Equals(exception, StringComparison.OrdinalIgnoreCase))
                            {
                                stack.Push(blueprints);
                                blueprints = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                            }
                        }

                        var memberName = MapOrDefault(reader.Name);
                        var property = properties.SingleOrDefault(pi => pi.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
                        if (property != null)
                        {
                            if (property.Name == nameof(Exception.InnerException) && reader.MoveToAttribute("namespace"))
                            {
                                objectType = Formatter.GetType($$"""{{reader.Value}}.{{exception}}""");
                                properties = objectType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                                blueprints.Add("Type", objectType);
                                blueprints.Add(memberName, null);
                            }
                            else
                            {
                                reader.Read();
                                blueprints.Add(memberName, reader.Value);
                            }
                        }
                        else
                        {
                            if (memberName == nameof(Exception.InnerException) && reader.MoveToAttribute("namespace"))
                            {
                                objectType = Formatter.GetType($$"""{{reader.Value}}.{{exception}}""");
                                properties = objectType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                                blueprints.Add("Type", objectType);
                                blueprints.Add(nameof(Exception.InnerException), null);
                            }
                        }
                        break;
                }

                lastException = exception;
            }

            if (blueprints.Count > 0) { stack.Push(blueprints); }

            Exception instance = null;

            var singleException = stack.Count == 1;

            while (stack.Count > 0)
            {
                var blueprint = stack.Pop();
                var desiredType = blueprint["type"] as Type;
                blueprint.Remove("type");
                if (singleException) { blueprint.Remove(nameof(Exception.InnerException)); }

                if (typeof(ArgumentException).IsAssignableFrom(objectType) && blueprint.ContainsKey("message"))
                {
#if NETSTANDARD2_0_OR_GREATER
                    blueprint["message"] = ((string)blueprint["message"]).Replace($"\nParameter name: {blueprint["paramName"]}", "");
#else
                    blueprint["message"] = ((string)blueprint["message"]).Replace($" (Parameter '{blueprint["paramName"]}')", "");
#endif
                }

                if (typeof(ArgumentOutOfRangeException).IsAssignableFrom(objectType) && blueprint.ContainsKey("message"))
                {
                    blueprint["message"] = ((string)blueprint["message"]).Replace($"\nActual value was {blueprint["actualValue"]}.", "");
                }

                if (blueprint.ContainsKey(nameof(Exception.InnerException)))
                {
                    blueprint[nameof(Exception.InnerException)] = instance;
                }

                var exceptionBaseCompatibleCtors = desiredType!.GetConstructors(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).Where(ci => ci.GetParameters().Any(pi => pi.ParameterType.IsAssignableFrom(typeof(Exception)) ||
                                                                                                                                                   pi.ParameterType.IsAssignableFrom(typeof(string)))).Reverse().ToList();

                var args = new List<object>();
                foreach (var ctor in exceptionBaseCompatibleCtors) // 1:1 match with constructor
                {
                    var ctorArgs = ctor.GetParameters();
                    var blueprintMatchLength = ctorArgs.Select(info => info.Name).Intersect(blueprint.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
                    if (ctorArgs.Length == blueprintMatchLength)
                    {
                        foreach (var arg in ctorArgs)
                        {
                            var kvp = blueprint.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase));
                            args.Add(Decorator.Enclose(arg.ParameterType).IsComplex()
                                ? kvp.Value
                                : Decorator.Enclose(kvp.Value).ChangeType(arg.ParameterType));
                            blueprint.Remove(arg.Name);
                        }
                        break;
                    }
                }

                if (args.Count == 0)
                {
                    foreach (var ctor in exceptionBaseCompatibleCtors) // partial match with constructor
                    {
                        var ctorArgs = ctor.GetParameters();
                        var blueprintMatchLength = ctorArgs.Select(info => info.Name).Count(ctorArgName => blueprint.Keys.Any(key => ctorArgName.EndsWith(key, StringComparison.OrdinalIgnoreCase)));
                        if (ctorArgs.Length == blueprintMatchLength)
                        {
                            foreach (var arg in ctorArgs)
                            {
                                var kvp = blueprint.First(pair => arg.Name.EndsWith(pair.Key, StringComparison.OrdinalIgnoreCase));
                                args.Add(Decorator.Enclose(kvp.Value).ChangeType(arg.ParameterType));
                                blueprint.Remove(kvp.Key);
                            }
                            break;
                        }
                    }
                }

                instance = Activator.CreateInstance(desiredType, args.ToArray()) as Exception;

                if (blueprint.Count > 0) { PopulateBlueprintToInstance(blueprint, desiredType, instance); }
            }

            return instance;
        }

        private static void PopulateBlueprintToInstance(Dictionary<string, object> blueprint, Type desiredType, object instance)
        {
            var blueprintIterator = new Dictionary<string, object>(blueprint);
            var fields = desiredType.GetFields(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
            var properties = desiredType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
            foreach (var kvp in blueprintIterator)
            {
                var property = properties.SingleOrDefault(pi => pi.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.CanWrite)
                {
                    property.SetValue(instance, Decorator.Enclose(kvp.Value).ChangeType(property.PropertyType));
                    blueprint.Remove(kvp.Key);
                }
                else // fallback to potential backing field
                {
                    var field = fields.SingleOrDefault(fi => fi.Name.EndsWith(kvp.Key, StringComparison.OrdinalIgnoreCase));
                    if (field != null)
                    {
                        field.SetValue(instance, Decorator.Enclose(kvp.Value).ChangeType(field.FieldType));
                        blueprint.Remove(kvp.Key);
                    }
                }
                if (blueprint.Count == 0) { break; }
            }
        }

        private static string MapOrDefault(string memberName)
        {
            switch (memberName.ToLowerInvariant())
            {
                case { } when memberName.EndsWith("Exception"):
                    return nameof(Exception.InnerException);
                case "Stack":
                    return nameof(Exception.StackTrace);
                default:
                    return memberName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <seealso cref="XmlConverter" /> can read XML.
        /// </summary>
        /// <value><c>true</c> if this <seealso cref="XmlConverter" /> can read XML; otherwise, <c>false</c>.</value>
        public override bool CanRead => true;

        private static void WriteExceptionCore(XmlWriter writer, Exception exception, bool includeStackTrace, bool includeData)
        {
            if (!string.IsNullOrEmpty(exception.Source))
            {
                writer.WriteElementString("Source", exception.Source);
            }

            if (!string.IsNullOrEmpty(exception.Message))
            {
                writer.WriteElementString("Message", exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WriteStartElement("Stack");
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    writer.WriteElementString("Frame", line.Trim());
                }
                writer.WriteEndElement();
            }

            if (includeData && exception.Data.Count > 0)
            {
                writer.WriteStartElement("Data");
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WriteStartElement(Decorator.Enclose(entry.Key.ToString()).SanitizeXmlElementName());
                    writer.WriteString(Decorator.Enclose(entry.Value?.ToString()).SanitizeXmlElementText());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>();
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                Decorator.Enclose(writer).WriteObject(value, value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(property.Name));
            }

            WriteInnerExceptions(writer, exception, includeStackTrace, includeData);
        }

        private static void WriteInnerExceptions(XmlWriter writer, Exception exception, bool includeStackTrace, bool includeData)
        {
            var innerExceptions = new List<Exception>();
            if (exception is AggregateException aggregated)
            {
                innerExceptions.AddRange(aggregated.Flatten().InnerExceptions);
            }
            else
            {
                if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            }
            if (innerExceptions.Count > 0)
            {
                var endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    var exceptionType = inner.GetType();
                    writer.WriteStartElement(Decorator.Enclose(exceptionType.Name).SanitizeXmlElementName());
                    if (exceptionType.Namespace != null) { writer.WriteAttributeString("namespace", exceptionType.Namespace); }
                    WriteExceptionCore(writer, inner, includeStackTrace, includeData);
                    endElementsToWrite++;
                }
                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndElement(); }
            }
        }
    }
}
