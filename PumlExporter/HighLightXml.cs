using System.Xml;

namespace PumlExporter;

public abstract class HighLightXml
{
    
    protected readonly XmlNamespaceManager NamespaceManager;
    protected readonly XmlDocument XmlDocument;


    protected HighLightXml(XmlDocument newXmlDocument, XmlNamespaceManager namespaceManager)
    {
        XmlDocument = newXmlDocument;
        NamespaceManager = namespaceManager;
    }

    public XmlDocument GetXmlDocument() => XmlDocument;
    private static void HighLightChildren(XmlElement node, bool newClass, ColorOptionsForElement colorOptions) // why static
    {
        if (newClass && node.Name == "rect")
        {
            node.SetAttribute("fill", colorOptions.RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", colorOptions.TextColor);
            node.SetAttribute("font-size", 14.ToString());
        }
    }
    public void HighLight(XmlDocument oldXmlDocument, ColorOptionsForElement options)
    {
        var newElements = XmlDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'elem_')]", NamespaceManager) ??
                          throw new Exception("new XmlDocument doesn't have element nodes");
        var oldElements = oldXmlDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'elem_')]", NamespaceManager) ??
                          throw new Exception("old XmlDocument doesn't have element nodes");

        foreach (XmlElement newElement in newElements)
        {
            var key = newElement.GetAttribute("id");

            var commonTextNodes = CommonNodes.GetCommonTextNodes(key, oldElements);

            if (commonTextNodes.Count != 0)
            {
                foreach (var child in newElement.Cast<XmlElement>().Where(n =>
                             !commonTextNodes.ContainsKey(n.InnerText.GetHashCode())))
                {
                    HighLightChildren(child, false, options);
                }
            }
            else
            {
                foreach (XmlElement newNode in newElement)
                {
                    HighLightChildren(newNode, true, options);
                }
            }
        }
    }
}