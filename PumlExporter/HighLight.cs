using System.Xml;

namespace PumlExporter;

public static class HighLight
{
    public static void ExportFile(RelativeFilePath updatedFilePath,
        params ColorOptions[] colorOptions)
    {
        var xmlDocument = SvgFile.GetXml(new RelativeFilePath("axon2.svg"));
        var nameSpaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
        nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
        nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
        var highLighter = new HighLightXml(xmlDocument, nameSpaceManager);
        //default value doesn't work
        highLighter.SvgGlobalHighLight("text",new Attribute("fill", "#383838"),
            new Attribute("font-size", "12"));

        if (colorOptions.Length != 0)
        {
            foreach (var colorOption in colorOptions)
            {
                if (colorOption.GetType() == typeof(ColorOptionsForElement))
                {
                    highLighter.SvgElementChangesHighLight(
                        SvgFile.GetXml(new RelativeFilePath("axon1.svg")), (ColorOptionsForElement)colorOption);
                }
            }
        }
        else
        {
            highLighter.SvgElementChangesHighLight(SvgFile.GetXml(new RelativeFilePath("axon1.svg")),
                new ColorOptionsForElement("#000000", "#C5CECE"));
        }

        highLighter.GetXmlDocument().Save(Path.Combine(updatedFilePath.Path));
    }
}