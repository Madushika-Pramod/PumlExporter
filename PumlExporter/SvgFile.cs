using System.Xml;

namespace PumlExporter;

public static class SvgFile
{
    public static XmlDocument GetXml(FilePath filePath)
    {
        var reader = new XmlTextReader(File.Open(filePath.Path, FileMode.Open));
        return GetXml(reader);
    }
    public static XmlDocument GetXml(string body)
    {
        var reader = new XmlTextReader(new StringReader(body));
        return GetXml(reader);
    }
    public static XmlDocument GetXml(XmlTextReader reader)
    {
        XmlDocument xmlDocument = new();
        xmlDocument.Load(reader);
        reader.Dispose();
        return xmlDocument;
    }
    
}