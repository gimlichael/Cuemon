using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Specifies options that is related to <see cref="ReflectionUtility.GetObjectHierarchy"/> operations.
    /// </summary>
    public class ObjectHierarchyOptions
    {
        private int _maxDepth;
        private int _maxCircularCalls;
        private Func<Type, bool> _skipPropertyType;
        private Func<PropertyInfo, bool> _skipProperty;
        private Func<object, bool> _hasCircularReference;
        private Func<ParameterInfo[], object[]> _propertyIndexParametersResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectHierarchyOptions"/> class.
        /// </summary>
        public ObjectHierarchyOptions()
        {
            MaxDepth = 10;
            MaxCircularCalls = 2;
            SkipPropertyType = source =>
            {
                switch (TypeCodeConverter.FromType(source))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Byte:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Empty:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.String:
                        return true;
                    default:
                        if (TypeUtility.IsKeyValuePair(source)) { return true; }
                        if (TypeUtility.ContainsType(source, typeof(MemberInfo))) { return true; }
                        return false;
                }
            };
            SkipProperty = property =>
            {
                return (property.PropertyType.GetTypeInfo().IsMarshalByRef ||
                        property.PropertyType.GetTypeInfo().IsSubclassOf(typeof(Delegate)) ||
                        property.Name.Equals("SyncRoot", StringComparison.Ordinal) ||
                        property.Name.Equals("IsReadOnly", StringComparison.Ordinal) ||
                        property.Name.Equals("IsFixedSize", StringComparison.Ordinal) ||
                        property.Name.Equals("IsSynchronized", StringComparison.Ordinal) ||
                        property.Name.Equals("Count", StringComparison.Ordinal) ||
                        property.Name.Equals("HResult", StringComparison.Ordinal) ||
                        property.Name.Equals("TargetSite", StringComparison.Ordinal));
            };
            HasCircularReference = ReflectionUtility.HasCircularReference;
            PropertyIndexParametersResolver = infos =>
            {
                List<object> resolvedParameters = new List<object>();
                for (int i = 0; i < infos.Length; i++)
                {
                    // because we don't know the values to pass to an indexer we will try to do some assumptions on a "normal" indexer
                    // however; this has it flaws: an indexer does not necessarily have an item on 0, 1, 2 etc., so must handle the possible
                    // TargetInvocationException.
                    // more info? check here: http://blog.nkadesign.com/2006/net-the-limits-of-using-reflection/comment-page-1/#comment-10813
                    if (TypeUtility.ContainsType(infos[i].ParameterType, typeof(Byte), typeof(Int16), typeof(Int32), typeof(Int64))) // check to see if we have a "normal" indexer
                    {
                        resolvedParameters.Add(0);
                    }
                }
                return resolvedParameters.ToArray();
            };
        }

        /// <summary>
        /// Gets or sets the maximum depth to safely traverse an object hierarchy. Default is 10.
        /// </summary>
        /// <value>The maximum depth to safely traverse an object hierarchy.</value>
        public int MaxDepth
        {
            get { return _maxDepth; }
            set
            {
                Validator.ThrowIfLowerThan(value, 0, nameof(value));
                _maxDepth = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum amount of times an object is allowed to make circular calls. Default is 2.
        /// </summary>
        /// <value>The maximum amount of times an object is allowed to make circular calls.</value>
        public int MaxCircularCalls
        {
            get { return _maxCircularCalls; }
            set
            {
                Validator.ThrowIfLowerThan(value, 0, nameof(value));
                _maxCircularCalls = value;
            }
        }

        /// <summary>
        /// Gets or sets the function delegate that is invoked just before public properties is being iterated and whose return <see cref="Type"/> determine if the properties should be skipped or not.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that determines if a given property <see cref="Type"/> should be skipped or not.</value>
        public Func<Type, bool> SkipPropertyType
        {
            get { return _skipPropertyType; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _skipPropertyType = value;
            }
        }

        /// <summary>
        /// Gets or sets the function delegate that is invoked every time a public property is iterated and whose <see cref="PropertyInfo"/> determine if that property should be skipped or not.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that determines if a given <see cref="PropertyInfo"/> should be skipped or not.</value>
        public Func<PropertyInfo, bool> SkipProperty
        {
            get { return _skipProperty; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _skipProperty = value;
            }
        }

        /// <summary>
        /// Gets or sets the function delegate that is invoked when a property has a value and whose return value suggest a circular reference.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that determines if an object is suggesting a circular reference.</value>
        public Func<object, bool> HasCircularReference
        {
            get { return _hasCircularReference; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _hasCircularReference = value;
            }
        }

        /// <summary>
        /// Gets or sets the function delegate that is invoked if a property has one or more index parameters.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that will resolve the index parameters of a <see cref="PropertyInfo"/>.</value>
        public Func<ParameterInfo[], object[]> PropertyIndexParametersResolver
        {
            get { return _propertyIndexParametersResolver; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _propertyIndexParametersResolver = value;
            }
        }
    }
}