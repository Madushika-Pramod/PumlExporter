using System.Collections;
using System.Xml;

namespace PumlExporter;

public class HighLightXml
{
    private bool _isChangesHighLighted;
    private readonly Selector _select;

    private readonly Dictionary<ObjectType, string> _backgroundNodes;
    private Dictionary<string, SvgAttribute[]> _options = null!;

    private HighLightXml(Builder builder)
    {
        _backgroundNodes = builder.BackgroundNodes;
        _select = builder.Select;
    }

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

    public void SvgChangesHighLight(XmlDocument oldDocument, IEnumerable<XmlObject> objects)
    {
        foreach (var obj in objects)
        {
            var newElements = _select.GetNodeList(obj.ObjectType);
            var oldElements = _select.GetNodeList(obj.ObjectType, oldDocument);

            //make a copy
            _options = new Dictionary<string, SvgAttribute[]>(obj.Options);
            if (!_options.Remove(_backgroundNodes[obj.ObjectType]))
            {
                throw new Exception("color option for background node is not set");
            }

            HighLightElement(obj.Options, newElements, oldElements);
        }

        _isChangesHighLighted = true;
    }

    public void SvgGlobalHighLight(string nodeType, params SvgAttribute[] attributes)
    {
        if (_isChangesHighLighted)
        {
            throw new Exception(
                "can't Globally high-light after high-lighted changes, consider apply this method before");
        }

        foreach (XmlElement node in _select.GetNodeList(nodeType))
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

    internal class Selector
    {
        private readonly XmlDocument _document;
        private XmlNamespaceManager _namespaceManager = null!;

        public Selector(XmlDocument document)
        {
            _document = document;
        }

        internal void SetNamespaceManager()
        {
            _namespaceManager = new XmlNamespaceManager(_document.NameTable);
            _namespaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
            _namespaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
        }

        public XmlNodeList GetNodeList(ObjectType objectType)
        {
            return GetNodeList(objectType, _document);
        }

        public XmlNodeList GetNodeList(ObjectType objectType, XmlDocument document)
        {
            if (_namespaceManager == null)
            {
                throw new Exception("name space manager is not set");
            }

            return document.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{objectType.ToText()}')]",
                       _namespaceManager) ??
                   throw new Exception("XmlDocument doesn't have element nodes");
        }

        public XmlNodeList GetNodeList(string nodeName)
        {
            if (_namespaceManager == null)
            {
                throw new Exception("name space manager is not set");
            }

            return _document.SelectNodes("descendant::s:" + nodeName, _namespaceManager) ??
                   throw new Exception("XmlDocument doesn't have element nodes");
        }
    }

    public class Builder
    {
        internal readonly Dictionary<ObjectType, string> BackgroundNodes = new();
        internal Selector Select = new(new XmlDocument());

        public Builder AddBackgroundNodes(ObjectType objectType, string nodeType)
        {
            BackgroundNodes.Add(objectType, nodeType);
            return this;
        }

        public Builder SetSelector(XmlDocument document)
        {
            Select = new Selector(document);
            Select.SetNamespaceManager();
            return this;
        }


        public HighLightXml Build() => new(this);
    }
}