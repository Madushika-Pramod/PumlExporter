using System.Xml;

namespace PumlExporter;

public static class Export
{
    public static void ExportFile(NewFile newFile, OldFile oldFile, RelativeFilePath updatedFilePath,
        params PumlType[] types)
    {
        var nameSpaceManager = new XmlNamespaceManager(oldFile.XmlDocument.NameTable);
        nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
        nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");


        if (types.Length == 0)
        {
            var type = new Elements("#000000", "#C5CECE");
            PumalDecorator.UpdateNewDocument(oldFile.GetNodeLists(type, nameSpaceManager),
                newFile.GetNodeLists(type, nameSpaceManager), type);
        }


        foreach (var objectType in types)
        {
            // _type = objectType;
            PumalDecorator.UpdateNewDocument(oldFile.GetNodeLists(objectType, nameSpaceManager),
                newFile.GetNodeLists(objectType, nameSpaceManager), objectType);
        }

        newFile.XmlDocument.Save(Path.Combine(updatedFilePath.Path));
    }
}