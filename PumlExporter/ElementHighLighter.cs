using System.Xml;

namespace PumlExporter;

public static class ElementHighLighter // can be static?
{

    private static void HighLightChilds(XmlElement node, bool newClass, ColorOptions options)
    {
        if (newClass && node.Name == "rect")
        {
            node.SetAttribute("fill", ((ElementColorOptions)options).RectColor);
        }
        else if (node.Name == "text")
        {
            node.SetAttribute("fill", ((ElementColorOptions)options).TextColor);
            node.SetAttribute("font-size", 14.ToString());
        }
    }
    private static void HighLightElements(XmlNodeList oldElements, XmlNodeList newElements, ElementColorOptions options)
    {
        // UpdateOldFileNodes(oldNodeList);

        foreach (XmlElement newElement in newElements)
        {
            // element Id
            var key = newElement.GetAttribute("id");

            var commonTextNodes = GetCommonTextNodes(key,oldElements);

            if (commonTextNodes.Count != 0)
            {
                foreach (var child in newElement.Cast<XmlElement>().Where(n =>
                             !commonTextNodes.ContainsKey(n.InnerText.GetHashCode())))
                {
                    HighLightChilds(child, false, options);
                }
            }
            else
            {
                foreach (XmlElement newNode in newElement)
                {
                    HighLightChilds(newNode, true, options);
                }
            }
        }
    }

    // private void UpdateNewDocument(PumlType type)
    // {
    //     
    //     UpdateNewDocument();
    // }

    public static void UpdateNewDocument(XmlNodeList? oldElements, XmlNodeList? newElements, ColorOptions colorOptions)
    {
        if (oldElements == null || newElements == null)
        {
            throw new Exception("old file or new file doesn't contain elements");
        }

        if (colorOptions.GetType() == typeof(ElementColorOptions))
        {
            HighLightElements(oldElements, newElements, (ElementColorOptions)colorOptions);
        }
        // else
        // {
        //     UpdateLInksOfNewFile(oldNodes, newNodes);// not implement
        // }
    }

   

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
    private static Dictionary<int, string> GetCommonTextNodes(string keyFromNewElement,XmlNodeList oldElements)
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