namespace PumlExporter;

public enum ObjectType
{
    Element,
    Link
}
public static class ObjectTypeExtensions
{
    public static string ToText(this ObjectType objectType) //todo this ObjectType
    {
        return objectType switch
        {
            ObjectType.Element => "elem_",
            ObjectType.Link => "link",
            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null)
        };
    }
}