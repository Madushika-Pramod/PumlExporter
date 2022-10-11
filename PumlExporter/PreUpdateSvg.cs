using System.Xml;

namespace PumlExporter;

public class PreUpdateSvg : HighLightXml
{
    public PreUpdateSvg(XmlDocument newXmlDocument, XmlNamespaceManager namespaceManager) : base(newXmlDocument, namespaceManager)
    {
    }
    public void UpdateText(params Attribute[] attributes)// this??
    {
        var nodes = XmlDocument.SelectNodes("descendant::s:text", NamespaceManager) ??
                    throw new Exception("XmlDocument doesn't have text nodes");
        foreach (XmlElement node in nodes)
        {
            foreach (var attribute in attributes)
            {
                node.SetAttribute(attribute.Parameter, attribute.Value);
            }
        }
        
        
    }
}