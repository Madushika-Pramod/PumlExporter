namespace PumlExporter;

public static class Export
{
    public static void ExportFile(RelativeFilePath updatedFilePath,
        params ColorOptions[] types)
    {
        var oldFile = new OldFile.Builder(new RelativeFilePath("axon1.svg")).SetElementsAndLinks(ObjectType.Element).Build();
        var newFile = new NewFile.Builder(new RelativeFilePath("axon2.svg")).Update().SetElementsAndLinks(ObjectType.Element)
            .Build();


        if (types.Length == 0)
        {
            types = new ColorOptions[]{new ElementColorOptions("#000000", "#C5CECE")};
            // var type = new Elements("#000000", "#C5CECE");
            // PumalDecorator.UpdateNewDocument(oldFile.Elements, newFile.Elements, type);
        }


        foreach (var objectType in types)
        {
            // _type = objectType;
            ElementHighLighter.UpdateNewDocument(oldFile.Elements, newFile.Elements, objectType);
        }

        newFile.XmlDocument.Save(Path.Combine(updatedFilePath.Path));
    }
}