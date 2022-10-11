using System.Xml;

namespace PumlExporter;

public class HighLightSvg: HighLightXml
{
    public HighLightSvg(XmlDocument newXmlDocument, XmlNamespaceManager namespaceManager) : base(newXmlDocument, namespaceManager)
    {
    }
}