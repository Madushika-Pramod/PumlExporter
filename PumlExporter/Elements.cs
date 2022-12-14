namespace PumlExporter;

public class Elements : PumlType
{
    public string TextColor { get; }
    public string RectColor { get; }

    public Elements(string textColor, string rectColor)
    {
        TextColor = textColor;
        RectColor = rectColor;
    }
    public override string ToString()
    {
        return "elem_";
    }
}