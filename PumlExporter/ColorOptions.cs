namespace PumlExporter;

public class ColorOptions
{
    public string TextColor { get; }
    public string RectColor { get; }

    public ColorOptions(string textColor, string rectColor)
    {
        TextColor = textColor;
        RectColor = rectColor;
    }
}