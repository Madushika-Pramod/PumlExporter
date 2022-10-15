using System.Collections;
using System.Xml;

namespace PumlExporter;

public class HighLightXml
{
    private bool _isChangesHighLighted;
    public readonly Dictionary<string, string> BackgroundNodes = new();
    private Dictionary<string, Attribute[]> _options = null!;
   
    private static void HighLightChildren(IEnumerable newElement, IReadOnlyDictionary<string, Attribute[]> options)
    {
        foreach (var node in newElement.Cast<XmlElement>()
                     .Where(node => options.ContainsKey(node.Name)))
        {
            SetAttribute(node, options[node.Name]);
        }
    }

    private void HighLightElement(IReadOnlyDictionary<string, Attribute[]> options, XmlNodeList newElements,
        XmlNodeList oldElements)
    {
        foreach (XmlElement newElement in newElements)
        {
            var key = newElement.GetAttribute("id");

            var commonTextNodes = CommonNodes.GetCommonTextNodes(key, oldElements);

            if (commonTextNodes.Count != 0)
            {
                //HighLight existing object
                HighLightChildren(
                    newElement.Cast<XmlElement>()
                        .Where(n => !commonTextNodes.ContainsKey(n.InnerText.GetHashCode())),
                    _options);
            }
            else
            {
                //HighLight new object
                HighLightChildren(newElement, options);
            }
        }
    }

    public void SvgChangesHighLight(XmlNodeList newElements, XmlNodeList oldElements,
        IEnumerable<XmlObject> objects)
    {
        foreach (var obj in objects)
        {
            _options = new Dictionary<string, Attribute[]>(obj.Options);
            if (!_options.Remove(BackgroundNodes[obj.ObjectType]))
            {
                throw new Exception("color option for background node is not set");
            }

            HighLightElement(obj.Options, newElements, oldElements);
        }

        _isChangesHighLighted = true;
    }
    
    public void SvgGlobalHighLight(XmlNodeList nodes, params Attribute[] attributes)
    {
        if (_isChangesHighLighted)
        {
            throw new Exception(
                "can't Globally high-light after high-lighted changes, consider apply this method before");
        }

        foreach (XmlElement node in nodes)
        {
            SetAttribute(node, attributes);
        }
    }

    private static void SetAttribute(XmlElement node, IEnumerable<Attribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            node.SetAttribute(attribute.Parameter, attribute.Value);
        }
    }
}