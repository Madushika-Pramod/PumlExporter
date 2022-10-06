using System.Xml;

namespace PumlExporter;

public class PumalDecorator // can be static?
{
    private PumlType _type = new Elements("#000000", "#C5CECE");
    // private XmlDocument _newDocument = new();
    // private static XmlDocument _oldDocument = new();

    private readonly Dictionary<string, XmlNodeList> _oldFileNodes = new();

    // private (XmlNodeList? oldNodeList, XmlNodeList? newNodeList) GetNodeLists(PumlType type)
    // {
    //     var oldNodeList =
    //         _oldDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type}')]",
    //             _nameSpaceManager);
    //     var newNodeList =
    //         _newDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type}')]",
    //             _nameSpaceManager);
    //
    //     return (oldNodeList, newNodeList);
    // }

    private void UpdateOldFileNodes(XmlNodeList oldNodeList)
    {
        foreach (XmlElement node in oldNodeList)
        {
            var key = node.GetAttribute("id");

            _oldFileNodes.Add(key, node.ChildNodes);
        }
    }

    private void HighLightNewElements(XmlNodeList oldNodeList, XmlNodeList newNodeList)
    {
        UpdateOldFileNodes(oldNodeList);

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

    // private void UpdateNewDocument(PumlType type)
    // {
    //     
    //     UpdateNewDocument();
    // }

    private void UpdateNewDocument(XmlNodeList? oldNodes, XmlNodeList? newNodes)
    {
        // var (oldNodes, newNodes) = GetNodeLists(_type);
        if (oldNodes == null || newNodes == null)
        {
            throw new Exception("old file or new file doesn't contain elements");
        }

        if (_type.GetType() == typeof(Elements))
        {
            HighLightNewElements(oldNodes, newNodes);
        }
        // else
        // {
        //     UpdateLInksOfNewFile(oldNodes, newNodes);// not implement
        // }
    }

    private void HighLightElements(XmlElement node, bool newClass)
    {
        if (newClass && node.Name == "rect")
        {
            node.SetAttribute("fill", ((Elements)_type).RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", ((Elements)_type).TextColor);
            node.SetAttribute("font-size", 14.ToString());
        }
    }

    private Dictionary<int, string> GetCommonMethods(string keyFromNewFile)
    {
        Dictionary<int, string> commonMethods = new();
        if (!_oldFileNodes.ContainsKey(keyFromNewFile))
        {
            return commonMethods;
        }


        foreach (XmlElement node in _oldFileNodes[keyFromNewFile])
        {
            if (node.Name == "text")
            {
                var _ = commonMethods.TryAdd(node.InnerText.GetHashCode(), node.InnerText);
            }
        }

        return commonMethods;
    }


    public void ExportFile(NewFile newFile, OldFile oldFile, RelativeFilePath updatedFilePath, params PumlType[] types)
    {
        var nameSpaceManager = new XmlNamespaceManager(oldFile.XmlDocument.NameTable);
        nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
        nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");


        if (types.Length == 0)
        {
            UpdateNewDocument(oldFile.GetNodeLists(_type, nameSpaceManager),
                newFile.GetNodeLists(_type, nameSpaceManager));
        }


        foreach (var objectType in types)
        {
            _type = objectType;
            UpdateNewDocument(oldFile.GetNodeLists(objectType, nameSpaceManager),
                newFile.GetNodeLists(objectType, nameSpaceManager));
        }
        newFile.XmlDocument.Save(Path.Combine(updatedFilePath.Path));
    }
}

