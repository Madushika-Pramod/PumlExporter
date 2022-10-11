using System.Xml;

namespace PumlExporter;

public class HighLightXml
{
    private readonly XmlNamespaceManager _namespaceManager;
    private readonly XmlDocument _xmlDocument;
    private bool _isChangesHighLighted;


    public HighLightXml(XmlDocument newXmlDocument, XmlNamespaceManager namespaceManager)
    {
        _xmlDocument = newXmlDocument;
        _namespaceManager = namespaceManager;
    }

    public XmlDocument GetXmlDocument() => _xmlDocument;
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
    public void SvgElementChangesHighLight(XmlDocument oldXmlDocument, ColorOptionsForElement options)
    {
        var newElements = _xmlDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'elem_')]", _namespaceManager) ??
                          throw new Exception("new XmlDocument doesn't have element nodes");
        var oldElements = oldXmlDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'elem_')]", _namespaceManager) ??
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

        _isChangesHighLighted = true;
    }
    public void SvgGlobalHighLight(string nodeName = "text",params Attribute[] attributes)// default parameter doesn't work??
    {
        if (_isChangesHighLighted)
        {
            throw new Exception("can't Globally high-light after high-lighted changes, consider apply this method before");
        }
        var nodes = _xmlDocument.SelectNodes("descendant::s:"+nodeName, _namespaceManager) ??
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