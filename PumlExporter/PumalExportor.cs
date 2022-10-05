using System.Xml;
namespace PumlExporter;

public class PumalExporter
{
    private readonly ColorOptions _colorOptions;
    private readonly string _lastFilePath;
    private readonly string _newFilePath;
    private readonly XmlDocument _newDocument = new();
    private static readonly XmlDocument OldDocument = new();
    private readonly XmlNamespaceManager _nameSpaceManager = new(OldDocument.NameTable);

    private readonly Dictionary<string, XmlNodeList> _nodesOfOldFile = new();


    public PumalExporter(ColorOptions colorOptions, string lastFilePath, string newFilePath )
    {
        _colorOptions = colorOptions;
        _lastFilePath = lastFilePath;
        _newFilePath = newFilePath;
    }


    private (XmlNodeList? oldNodeList, XmlNodeList? newNodeList) GetNodesOfBothFiles(ObjectType type,
        XmlDocument oldDocument,
        XmlNamespaceManager nameSpaceManager)
    {
        var oldNodeList =
            oldDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type.ToText()}')]",
                nameSpaceManager);
        var newNodeList =
            _newDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type.ToText()}')]",
                nameSpaceManager);

        return (oldNodeList, newNodeList);
    }

    private void ImportFile()
    {
        var files = new FileInput(_lastFilePath, _newFilePath);
        OldDocument.Load(files.OldDataReader);
        _newDocument.Load(files.NewDataReader);
        _nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
        _nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
    }

    private void GetNodesOfOldFile(XmlNodeList oldNodeList)
    {
        foreach (XmlElement node in oldNodeList)
        {
            var key = node.GetAttribute("id");

            _nodesOfOldFile.Add(key, node.ChildNodes);
        }
    }


    private void GetUpdatedElementsOfNewFile(XmlNodeList oldNodeList, XmlNodeList newNodeList)
    {
        GetNodesOfOldFile(oldNodeList);

        foreach (XmlElement nodeOfNewFile in newNodeList)
        {
            // element Id
            var key = nodeOfNewFile.GetAttribute("id");

            var commonMethods = GetCommonMethods(key);

            if (commonMethods.Count != 0)
            {
                foreach (var node in nodeOfNewFile.Cast<XmlElement>().Where(n =>
                             !commonMethods.ContainsKey(n.InnerText.GetHashCode())))
                {
                    HighLightElements(node, false);
                }
            }
            else
            {
                foreach (XmlElement newNode in nodeOfNewFile)
                {
                    HighLightElements(newNode, true);
                }
            }
        }
    }

    private void UpdateNewFile(ObjectType objectType)
    {
        var (oldNodes, newNodes) = GetNodesOfBothFiles(objectType, OldDocument, _nameSpaceManager);
        if (oldNodes == null || newNodes == null) return;
        if (objectType == ObjectType.Element)
        {
            GetUpdatedElementsOfNewFile(oldNodes, newNodes);
        }
        // else
        // {
        //     UpdateLInksOfNewFile(oldNodes, newNodes);
        // }
    }

    private void HighLightElements(XmlElement node, bool newClass)
    {
        if (newClass && node.Name == "rect")
        {
            node.SetAttribute("fill", _colorOptions.RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", _colorOptions.TextColor);
            node.SetAttribute("font-size", 14.ToString());
        }
    }

    private Dictionary<int, string> GetCommonMethods(string keyFromNewFile)
    {
        Dictionary<int, string> commonMethods = new();
        if (!_nodesOfOldFile.ContainsKey(keyFromNewFile))
        {
            return commonMethods;
        }


        foreach (XmlElement node in _nodesOfOldFile[keyFromNewFile])
        {
            if (node.Name == "text")
            {
                var _ = commonMethods.TryAdd(node.InnerText.GetHashCode(), node.InnerText);
            }
        }

        return commonMethods;
    }


    public void ExportFile(string updatedFilePath)
    {
        ImportFile();
        UpdateNewFile(ObjectType.Element);
        //add more
        _newDocument.Save(Path.Combine(updatedFilePath));
    }
}