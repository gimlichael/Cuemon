// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy implementation.", Scope = "member", Target = "~M:Cuemon.Xml.Serialization.Converters.DefaultXmlConverter.ParseReadXmlDefault(System.Xml.XmlReader,System.Type)~System.Object")]
[assembly: SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Xml.XmlStreamFactory.CreateStream(System.Action{System.Xml.XmlWriter},System.Action{System.Xml.XmlWriterSettings})~System.IO.Stream")]
[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy implementation.", Scope = "member", Target = "~M:Cuemon.Xml.XmlReaderDecoratorExtensions.BuildHierarchy(System.Xml.XmlReader)~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "Readability; at least for me.", Scope = "member", Target = "~M:Cuemon.Xml.Serialization.Converters.DefaultXmlConverter.SkipIfNullOrEmptyEnumerable(Cuemon.IHierarchy{System.Object})~System.Boolean")]
[assembly: SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "By design; to provide XmlIgnoreAttribute.", Scope = "member", Target = "~P:Cuemon.Xml.Serialization.XmlWrapper.InstanceType")]
[assembly: SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "By design; to provide XmlIgnoreAttribute.", Scope = "member", Target = "~P:Cuemon.Xml.Serialization.XmlWrapper.MemberReference")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Xml")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Xml.Linq")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Xml.Serialization")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Xml.Serialization.Converters")]
