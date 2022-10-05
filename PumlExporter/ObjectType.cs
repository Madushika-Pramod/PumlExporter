namespace PumlExporter;

public enum ObjectType
{   Element,
    Link
}

internal static class ObjectTypeExtensions
{
    public static string ToText(this ObjectType objectType)
    {
        return objectType switch
        {
            ObjectType.Element => "elem_",
            ObjectType.Link => "link_",
            _ => throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null)
        };
    } 
    
}