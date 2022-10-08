using System.Xml;

namespace PumlExporter;

public class HighLight
{
    private readonly ColorOptionsForElement _options;

    public HighLight(ColorOptionsForElement options)
    {
        _options = options;
    }

    private void HighLightChildren(XmlElement node, bool newClass, ColorOptions options)
    {
        if (newClass && node.Name == "rect")
        {
            node.SetAttribute("fill", ((ColorOptionsForElement)options).RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", ((ColorOptionsForElement)options).TextColor);
            node.SetAttribute("font-size", 14.ToString());
        }
    }

    public void HighLightElements(XmlNodeList oldElements, XmlNodeList newElements)
    {
        foreach (XmlElement newElement in newElements)
        {
            // element Id
            var key = newElement.GetAttribute("id");

            var commonTextNodes = CommonNodes.GetCommonTextNodes(key, oldElements);

            if (commonTextNodes.Count != 0)
            {
                foreach (var child in newElement.Cast<XmlElement>().Where(n =>
                             !commonTextNodes.ContainsKey(n.InnerText.GetHashCode())))
                {
                    HighLightChildren(child, false, _options);
                }
            }
            else
            {
                foreach (XmlElement newNode in newElement)
                {
                    HighLightChildren(newNode, true, _options);
                }
            }
        }
    }
}