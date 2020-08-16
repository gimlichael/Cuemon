// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy implementation.", Scope = "member", Target = "~M:Cuemon.Xml.Serialization.Converters.DefaultXmlConverter.ParseReadXmlDefault(System.Xml.XmlReader,System.Type)~System.Object")]
[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy implementation.", Scope = "member", Target = "~M:Cuemon.Xml.XmlDataReader.ReadNext(System.Boolean)~System.Boolean")]
[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy implementation.", Scope = "member", Target = "~M:Cuemon.Xml.XmlReaderDecoratorExtensions.ToHierarchy(Cuemon.IDecorator{System.Xml.XmlReader})~Cuemon.IHierarchy{Cuemon.DataPair}")]
[assembly: SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "False-positive.", Scope = "member", Target = "~M:Cuemon.Xml.XmlStreamFactory.CreateStream(System.Action{System.Xml.XmlWriter},System.Action{System.Xml.XmlWriterSettings})~System.IO.Stream")]
