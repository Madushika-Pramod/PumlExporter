namespace PumlExporter;

public class XmlObject
{
    public readonly Dictionary<string, SvgAttribute[]> Options;
    public readonly string BackgroundNode;
    public readonly ObjectType ObjectType;
    

    public XmlObject(Dictionary<string, SvgAttribute[]> options,string backgroundNode, ObjectType objectType)
    {
        Options = options;
        BackgroundNode = backgroundNode;
        ObjectType = objectType;
    }
}