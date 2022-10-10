using System.Xml;
using PumlExporter.HighLightExtensions;

namespace PumlExporter;

public static class Export
{
    public static void ExportFile(RelativeFilePath updatedFilePath,
        params ColorOptions[] colorOptions)
    {
        var xmlDocument = SvgFile.GetXmlDocument(new RelativeFilePath("axon2.svg"));
        var nameSpaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
        nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
        nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
        var highLighter = new HighLight(xmlDocument, nameSpaceManager);
        highLighter.UpdateTextElements(new Attribute("fill", "#383838"),
            new Attribute("font-size", "12"));
        
        if (colorOptions.Length != 0)
        {
            foreach (var colorOption in colorOptions)
            {
                if (colorOption.GetType() == typeof(ColorOptionsForElement))
                {
                    highLighter.HighLightNewElements(SvgFile.GetXmlDocument(new RelativeFilePath("axon1.svg")),
                        (ColorOptionsForElement)colorOption);
                }
            }
        }
        else
        {
            highLighter.HighLightNewElements(SvgFile.GetXmlDocument(new RelativeFilePath("axon1.svg")),new ColorOptionsForElement("#000000","#C5CECE"));
        }
        
        highLighter.XmlDocument.Save(Path.Combine(updatedFilePath.Path));
    }
}