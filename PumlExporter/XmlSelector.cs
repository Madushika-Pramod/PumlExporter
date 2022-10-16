// using System.Xml;
// namespace PumlExporter;
// public static class XmlSelector
// {
//     public static XmlNodeList GetObjectNodeList(XmlDocument xmlDocument, XmlNamespaceManager manager, string objectType)
//     {
//         return xmlDocument.SelectNodes( $"//s:g[1]/s:g[starts-with(@id,'{objectType}')]", manager) ??
//                throw new Exception("XmlDocument doesn't have element nodes");
//     }
//
//     public static XmlNodeList GetGlobalNodeList(XmlDocument xmlDocument, XmlNamespaceManager manager, string nodeName)
//     {
//         return xmlDocument.SelectNodes("descendant::s:" + nodeName, manager) ??
//                throw new Exception("XmlDocument doesn't have element nodes");
//     }
// }