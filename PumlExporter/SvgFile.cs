using System.Xml;

namespace PumlExporter;

public static class SvgFile
{
    public static XmlDocument GetXmlDocument(RelativeFilePath filePath)
    {
        XmlDocument xmlDocument = new();
        var reader = new XmlTextReader(File.Open(filePath.Path, FileMode.Open));
        xmlDocument.Load(reader);
        return xmlDocument;
    }
}