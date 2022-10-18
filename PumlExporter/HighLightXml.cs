using System.Collections;
using System.Xml;

namespace PumlExporter;

public class HighLightXml
{
    private bool _isChangesHighLighted;
    private readonly Selector _selector = new();

    private Dictionary<string, SvgAttribute[]> _options = null!;

    //todo what is this conditional-> NewDocument?
    public XmlDocument? GetHighLightedFile() => _selector.NewDocument;

    private static void HighLightChildren(IEnumerable newElement, IReadOnlyDictionary<string, SvgAttribute[]> options)
    {
        foreach (var node in newElement.Cast<XmlElement>()
                     .Where(node => options.ContainsKey(node.Name)))
        {
            SetAttribute(node, options[node.Name]);
        }
    }

    private void HighLightElement(IReadOnlyDictionary<string, SvgAttribute[]> options, XmlNodeList newElements,
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

    public void SvgChangesHighLight(string oldSvg, IEnumerable<XmlObject> objects)
    {
        SvgChangesHighLight(oldSvg, null, objects);
    }

    // shouldn't make private
    public void SvgChangesHighLight(string oldSvg, string? newSvg, IEnumerable<XmlObject> objects)
    {
        if (_selector.NewDocument != null && newSvg != null)
        {
            newSvg = null;
        }
        else if (_selector.NewDocument == null && newSvg == null)
        {
            throw new Exception("both old file path and new file path should be provided");
        }

        foreach (var obj in objects)
        {
            var newElements = _selector.GetNodeList(obj.ObjectType, newSvg);
            var oldElements = _selector.GetNodeList(obj.ObjectType, oldSvg);

            //make a copy
            _options = new Dictionary<string, SvgAttribute[]>(obj.Options);
            if (!_options.Remove(obj.BackgroundNode))
            {
                throw new Exception("color option for background node is not set");
            }

            HighLightElement(obj.Options, newElements, oldElements);
        }

        _isChangesHighLighted = true;
    }

    public void SvgGlobalHighLight(string nodeType, string newSvg, params SvgAttribute[] attributes)
    {
        if (_isChangesHighLighted)
        {
            throw new Exception(
                "can't Globally high-light after high-lighted changes, consider apply this method before");
        }
        foreach (XmlElement node in _selector.GetNodeList(newSvg, nodeType))
        {
            SetAttribute(node, attributes);
        }
        
    }
    private static void SetAttribute(XmlElement node, IEnumerable<SvgAttribute> attributes)
    {
        foreach (var attribute in attributes)
        {
            node.SetAttribute(attribute.Parameter, attribute.Value);
        }
    }

    private static class PumlFile
    {
        public static XmlDocument GetXmlByText(string body)
        {
            var reader = new XmlTextReader(new StringReader(body));
            return GetXml(reader);
        }

        private static XmlDocument GetXml(XmlReader reader)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.Load(reader);
            reader.Dispose();
            return xmlDocument;
        }
    }

    private class Selector
    {
        private XmlNamespaceManager? _namespaceManager;

        public XmlDocument? NewDocument;

        private void SetNamespaceManager(XmlDocument document)
        {
            _namespaceManager = new XmlNamespaceManager(document.NameTable);
            _namespaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
            _namespaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
        }

        public void ResetSector()
        {
            NewDocument = null;
            _namespaceManager = null;
        }

        private XmlDocument GetPumlNodeList(string svgBody)
        {
            var xml = PumlFile.GetXmlByText(svgBody);
            if (_namespaceManager == null)
            {
                SetNamespaceManager(xml);
            }
            
            var doc = xml.SelectNodes($"//s:g[1]/s:g[1]", _namespaceManager!)?[0];
            if (doc is { Name: "g" } &&
                doc.OuterXml.StartsWith("<g id=\"elem_") &&
                doc.OwnerDocument?.Implementation.ToString() == "System.Xml.XmlImplementation" &&
                doc.OwnerDocument.DocumentElement?.Name == "svg")
            {
                NewDocument ??= xml;
                return xml;
            }

            _namespaceManager = null;
            throw new Exception("Invalid Puml file");
        }

        private XmlNodeList GetPumlNodeList(ObjectType objectType, XmlDocument document)
        {
            return document.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{objectType.ToText()}')]",
                _namespaceManager!) ?? throw new Exception("Invalid Puml file");
        }

        public XmlNodeList GetNodeList(string svgBody, string nodeName)
        {
            return GetNodeList(GetPumlNodeList(svgBody), nodeName);

        }

        private XmlNodeList GetNodeList(XmlDocument newDocument, string nodeName)
        {
            if (NewDocument != null)
                return NewDocument.SelectNodes("descendant::s:" + nodeName, _namespaceManager!) ??
                       throw new Exception("Invalid Puml file");
            
            return newDocument.SelectNodes("descendant::s:" + nodeName, _namespaceManager!) ??
                   throw new Exception("Invalid Puml file");
        }

        public XmlNodeList GetNodeList(ObjectType objectType, string? svgBody)
        {
            if (svgBody == null)
            {
                // new doc is set by global high lighter
                if (NewDocument != null)
                {
                    return GetPumlNodeList(objectType, NewDocument);
                }

                throw new Exception("Svg file is not provided");
            }

            if (_namespaceManager != null && NewDocument != null)
            {
                // old list
                return GetPumlNodeList(objectType, GetPumlNodeList(svgBody));
            }

            // new list
            return GetPumlNodeList(objectType, GetPumlNodeList(svgBody));
        }
    }
}