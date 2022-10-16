namespace PumlExporter;

public class XmlObject
{
    public readonly Dictionary<string, SvgAttribute[]> Options;
    public readonly ObjectType ObjectType;

    public XmlObject(Dictionary<string, SvgAttribute[]> options, ObjectType objectType)
    {
        Options = options;
        ObjectType = objectType;
    }
}