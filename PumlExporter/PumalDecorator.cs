using System.Xml;
namespace PumlExporter;

public static class PumalDecorator // can be static?
{

    private static readonly Dictionary<string, XmlNodeList> OldFileNodes = new();
    
    private static void UpdateOldFileNodes(XmlNodeList oldNodeList)
    {
        foreach (XmlElement node in oldNodeList)
        {
            var key = node.GetAttribute("id");

            OldFileNodes.Add(key, node.ChildNodes);
        }
    }

    private static void HighLightNewElements(XmlNodeList oldNodeList, XmlNodeList newNodeList,PumlType type)
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
                    HighLightElements(node, false,type);
                }
            }
            else
            {
                foreach (XmlElement newNode in nodeOfNewFile)
                {
                    HighLightElements(newNode, true,type);
                }
            }
        }
    }

    // private void UpdateNewDocument(PumlType type)
    // {
    //     
    //     UpdateNewDocument();
    // }

    public static void UpdateNewDocument(XmlNodeList? oldNodes, XmlNodeList? newNodes,PumlType type )
    {
        if (oldNodes == null || newNodes == null)
        {
            throw new Exception("old file or new file doesn't contain elements");
        }

        if (type.GetType() == typeof(Elements))
        {
            HighLightNewElements(oldNodes, newNodes,type);
        }
        // else
        // {
        //     UpdateLInksOfNewFile(oldNodes, newNodes);// not implement
        // }
    }

    private static void HighLightElements(XmlElement node, bool newClass,PumlType type)
    {
        if (newClass && node.Name == "rect")
        {
            node.SetAttribute("fill", ((Elements)type).RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", ((Elements)type).TextColor);
            node.SetAttribute("font-size", 14.ToString());
        }
    }

    private static Dictionary<int, string> GetCommonMethods(string keyFromNewFile)
    {
        Dictionary<int, string> commonMethods = new();
        if (!OldFileNodes.ContainsKey(keyFromNewFile))
        {
            return commonMethods;
        }


        foreach (XmlElement node in OldFileNodes[keyFromNewFile])
        {
            if (node.Name == "text")
            {
                var _ = commonMethods.TryAdd(node.InnerText.GetHashCode(), node.InnerText);
            }
        }

        return commonMethods;
    }
}

