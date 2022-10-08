using System.Xml;

namespace PumlExporter;

public static class Update
{
    public static void UpdateFile(XmlNodeList? oldElements, XmlNodeList? newElements, ColorOptions colorOptions)
    {
        if (oldElements == null || newElements == null)
        {
            throw new Exception("old file or new file doesn't contain elements");
        }

        if (colorOptions.GetType() == typeof(ColorOptionsForElement))
        {
            var highLighter = new HighLight((ColorOptionsForElement)colorOptions);
            highLighter.HighLightElements(oldElements, newElements);
        }
        // else
        // {
        //     UpdateLInksOfNewFile(oldNodes, newNodes);// not implement
        // }
    }
}