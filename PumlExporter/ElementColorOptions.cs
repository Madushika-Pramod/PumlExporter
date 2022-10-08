namespace PumlExporter;

public class ElementColorOptions : ColorOptions
{
    public string TextColor { get; }
    public string RectColor { get; }

    public ElementColorOptions(string textColor, string rectColor)
    {
        TextColor = textColor;
        RectColor = rectColor;
    }
    
}