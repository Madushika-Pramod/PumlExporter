using System.Xml;

namespace PumlExporter;

public class HighLightXml
{
    private bool _isChangesHighLighted;

    private static void
        HighLightChildren(XmlElement node, bool newClass, ColorOptionsForElement colorOptions) // why static
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

    public void SvgElementChangesHighLight(XmlNodeList newElements,XmlNodeList oldElements, ColorOptionsForElement options)
    {

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

    public void SvgGlobalHighLight(XmlNodeList nodes, params Attribute[] attributes) // default parameter doesn't work??
    {
        if (_isChangesHighLighted)
        {
            throw new Exception(
                "can't Globally high-light after high-lighted changes, consider apply this method before");
        }
        foreach (XmlElement node in nodes)
        {
            foreach (var attribute in attributes)
            {
                node.SetAttribute(attribute.Parameter, attribute.Value);
            }
        }
    }
}
