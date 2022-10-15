namespace PumlExporter;

public class XmlObject
{
    public readonly Dictionary<string, Attribute[]> Options;
    public readonly string ObjectType;

    public XmlObject(Dictionary<string, Attribute[]> options, string objectType)
    {
        Options = options;
        ObjectType = objectType;
    }
}