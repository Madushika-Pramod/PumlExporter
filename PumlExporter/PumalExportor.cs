using System.Xml;

namespace PumlExporter;

public class PumalExporter
{
    private PumlObject _objectType = new Elements("#000000", "#C5CECE");
    private readonly string _lastFilePath;
    private readonly string _newFilePath;
    private readonly XmlDocument _newDocument = new();
    private static readonly XmlDocument OldDocument = new();
    private readonly XmlNamespaceManager _nameSpaceManager = new(OldDocument.NameTable);

    private readonly Dictionary<string, XmlNodeList> _nodesOfOldFile = new();


    public PumalExporter(string lastFilePath, string newFilePath)
    {
        _lastFilePath = lastFilePath;
        _newFilePath = newFilePath;
    }


    private (XmlNodeList? oldNodeList, XmlNodeList? newNodeList) GetNodesOfBothFiles(PumlObject type,
        XmlDocument oldDocument,
        XmlNamespaceManager nameSpaceManager)
    {
        var oldNodeList =
            oldDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type}')]",
                nameSpaceManager);
        var newNodeList =
            _newDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type}')]",
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

    private void UpdateNewFile(PumlObject objectType)
    {
        _objectType = objectType;
        UpdateNewFile();
    }

    private void UpdateNewFile()
    {
        var (oldNodes, newNodes) = GetNodesOfBothFiles(_objectType, OldDocument, _nameSpaceManager);
        if (oldNodes == null || newNodes == null) return;
        if (_objectType.ToString() == "elem_")
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
            node.SetAttribute("fill", ((Elements)_objectType).RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", ((Elements)_objectType).TextColor);
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


    public void ExportFile(string updatedFilePath, params PumlObject[] types)
    {
        ImportFile();
        if (types.Length == 0)
        {
            UpdateNewFile();
        }

        foreach (var objectType in types)
        {
            UpdateNewFile(objectType);
        }

        _newDocument.Save(Path.Combine(updatedFilePath));

        //add more
    }

     
}