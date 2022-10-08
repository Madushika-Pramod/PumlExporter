namespace PumlExporter;

public class ColorOptionsForElement : ColorOptions
{
    public string TextColor { get; }
    public string RectColor { get; }

    public ColorOptionsForElement(string textColor, string rectColor)
    {
        TextColor = textColor;
        RectColor = rectColor;
    }
    
}