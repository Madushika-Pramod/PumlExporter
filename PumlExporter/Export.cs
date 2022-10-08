namespace PumlExporter;

public static class Export
{
    public static void ExportFile(RelativeFilePath updatedFilePath,
        params ColorOptions[] colorOptions)
    {
        var oldFile = new OldSvgFile.Builder(new RelativeFilePath("axon1.svg")).SetElements(ObjectType.Element) // type should be removed
            .Build();
        var newFile = new NewSvgFile.Builder(new RelativeFilePath("axon2.svg")).Update()
            .SetElements(ObjectType.Element)
            .Build();
        
        if (colorOptions.Length == 0)
        {
            colorOptions = new ColorOptions[] { new ColorOptionsForElement("#000000", "#C5CECE") };
        }
        
        foreach (var objectType in colorOptions)
        {
            Update.UpdateFile(oldFile.Elements, newFile.Elements, objectType);
        }

        newFile.XmlDocument.Save(Path.Combine(updatedFilePath.Path));
    }
}