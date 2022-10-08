using System.Xml;

namespace PumlExporter;

public static class CommonNodes
{
   // get dictionary key: element Id, value: ChildNodes of element
    private static Dictionary<string, XmlNodeList> GetChildNodes(XmlNodeList elements)
    {
        var childNodes = new Dictionary<string, XmlNodeList>();
        foreach (XmlElement element in elements)
        {
            var key = element.GetAttribute("id");

            childNodes.Add(key, element.ChildNodes);
        }

        return childNodes;
    }
    public static Dictionary<int, string> GetCommonTextNodes(string keyFromNewElement,XmlNodeList oldElements)
    {
        var commonTextNodes = new Dictionary<int, string>();
        var oldChildNodes = GetChildNodes(oldElements);
        if (!oldChildNodes.ContainsKey(keyFromNewElement))
        {
            return commonTextNodes;
        }


        foreach (XmlElement node in oldChildNodes[keyFromNewElement])
        {
            if (node.Name == "text")
            {
                var _ = commonTextNodes.TryAdd(node.InnerText.GetHashCode(), node.InnerText);
            }
        }

        return commonTextNodes;
    }

}