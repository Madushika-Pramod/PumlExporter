using System.Xml;

namespace PumlExporter.HighLightExtensions;

public static class HighLightExtensions
{
    public static void UpdateTextElements(this HighLight highLight,params Attribute[] attributes)// this??
    {
        var nodes = highLight.XmlDocument.SelectNodes("descendant::s:text", highLight.NamespaceManager) ??
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