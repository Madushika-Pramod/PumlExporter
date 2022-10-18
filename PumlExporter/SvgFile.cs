using System.Xml;

namespace PumlExporter;

// public static class SvgFile
// {
//     // public static XmlDocument GetXmlByFilePath(string filePath)
//     // {
//     //         var reader = new XmlTextReader(File.Open(filePath, FileMode.Open));
//     //     return GetXml(reader);
//     // }
//     public static XmlDocument GetXmlByText(string body)
//     {
//         var reader = new XmlTextReader(new StringReader(body));
//         return GetXml(reader);
//     }
//
//     private static XmlDocument GetXml(XmlReader reader)
//     {
//         XmlDocument xmlDocument = new();
//         xmlDocument.Load(reader);
//         reader.Dispose();
//         return xmlDocument;
//     }
//     
// }