using System.Xml;

namespace PumlExporter;

public static class HighLight
{
    public static void Save(RelativeFilePath updatedFilePath,
        params ColorOptions[] colorOptions)
    {
        var oldXml = SvgFile.GetXml(new RelativeFilePath("axon1.svg"));
        var newXml = SvgFile.GetXml(new RelativeFilePath("axon2.svg"));
        var nameSpaceManager = new XmlNamespaceManager(oldXml.NameTable);
        var highLighter = new HighLightXml();

        void SvgElementChangesHighLight(ColorOptionsForElement colorOption)
        {
            highLighter!.SvgElementChangesHighLight(
                XmlSelector.GetObjectNodeList(newXml!, nameSpaceManager!, "elem_"),
                XmlSelector.GetObjectNodeList(oldXml!, nameSpaceManager!, "elem_"),
                colorOption);
        }

        nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
        nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

        //default value doesn't work
        highLighter.SvgGlobalHighLight(XmlSelector.GetGlobalNodeList(newXml, nameSpaceManager, "text"),
            new Attribute("fill", "#383838"),
            new Attribute("font-size", "12"));


        if (colorOptions.Length != 0)
        {
            // Color Options For objects(elems & links)
            foreach (var colorOption in colorOptions)
            {
                if (colorOption.GetType() == typeof(ColorOptionsForElement))
                {
                    SvgElementChangesHighLight((ColorOptionsForElement)colorOption);
                }
            }
        }
        else
        {
            SvgElementChangesHighLight(new ColorOptionsForElement("#000000", "#C5CECE"));
        }

        newXml.Save(Path.Combine(updatedFilePath.Path));
    }
}