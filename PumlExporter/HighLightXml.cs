using System.Collections;
using System.Xml;

namespace PumlExporter;

public class HighLightXml
{
    private bool _isChangesHighLighted;
    private readonly Selector _selector = new();
    private readonly Dictionary<ObjectType, string> _backgroundNodes = new();

    private Dictionary<string, SvgAttribute[]> _options = null!;

    //todo what is this conditional-> NewDocument?
    public void SaveFile(string path) => _selector.NewDocument?.Save(new FilePath(path).Path);

    public void AddBackgroundNodes(ObjectType objectType, string nodeType) =>
        _backgroundNodes.Add(objectType, nodeType);

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

    public void SvgChangesHighLight(string oldFilePath, IEnumerable<XmlObject> objects)
    {
        SvgChangesHighLight(oldFilePath, null, objects);
    }

    public void SvgChangesHighLight(string oldFilePath, string? newFilePath, IEnumerable<XmlObject> objects)
    {
        if (_selector.NewDocument != null && newFilePath != null)
        {
            newFilePath = null;
        }
        else if (_selector.NewDocument == null && newFilePath == null)
        {
            throw new Exception("both old file path and new file path should be provided");
        }

        foreach (var obj in objects)
        {
            var newElements = _selector.GetNodeList(obj.ObjectType, newFilePath);
            var oldElements = _selector.GetNodeList(obj.ObjectType, oldFilePath);

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

    public void SvgGlobalHighLight(string nodeType, string filePath, params SvgAttribute[] attributes)
    {
        SvgGlobalHighLight(nodeType, SvgFile.GetXml(new FilePath(filePath)), attributes);
    }

    public void SvgGlobalHighLight(string nodeType, XmlDocument newDocument, params SvgAttribute[] attributes)
    {
        if (_isChangesHighLighted)
        {
            throw new Exception(
                "can't Globally high-light after high-lighted changes, consider apply this method before");
        }

        _selector.NewDocument = newDocument;
        _selector.SetNamespaceManager(newDocument);

        foreach (XmlElement node in _selector.GetNodeList(nodeType))
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

    private class Selector
    {
        private XmlNamespaceManager? _namespaceManager;

        public XmlDocument? NewDocument;
        // private XmlDocument? _oldDocument;

        public void SetNamespaceManager(XmlDocument document)
        {
            _namespaceManager = new XmlNamespaceManager(document.NameTable);
            _namespaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
            _namespaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
        }

        private XmlNodeList GetNodeList(ObjectType objectType, XmlDocument document)
        {
            return document.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{objectType.ToText()}')]",
                       _namespaceManager!) ??
                   throw new Exception("XmlDocument doesn't have element nodes");
        }

        public XmlNodeList GetNodeList(string nodeName)
        {
            if (NewDocument != null)
                return NewDocument.SelectNodes("descendant::s:" + nodeName, _namespaceManager!) ??
                       throw new Exception("XmlDocument doesn't have element nodes");
            throw new Exception("XmlDocument is null");
        }

        public XmlNodeList GetNodeList(ObjectType objectType, string? filePath)
        {
            if (filePath == null)
            {
                if (NewDocument != null)
                {
                    return GetNodeList(objectType, NewDocument);
                }

                throw new Exception("file path is not provided");
            }

            if (_namespaceManager != null && NewDocument != null)
                return GetNodeList(objectType, SvgFile.GetXml(new FilePath(filePath)));
            
            NewDocument = SvgFile.GetXml(new FilePath(filePath));
            SetNamespaceManager(NewDocument);
            return GetNodeList(objectType, NewDocument);

        }
    }
}