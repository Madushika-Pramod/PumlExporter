using PumlExporter;
var options = new Dictionary<string, SvgAttribute[]>
{
    {
        "text",
        new[] { new SvgAttribute("fill", "#000000"), new SvgAttribute("font-size", "14") }
    },
    {
        "rect", new[] { new SvgAttribute("fill", "#C5CECE") }
    }
};
var xmlObject = new XmlObject(options,"rect",ObjectType.Element);
var highLighter = new HighLightXml();

highLighter.SvgGlobalHighLight("text","../../../axon2.svg",
    new SvgAttribute("fill", "#383838"),
    new SvgAttribute("font-size", "12"));

highLighter.SvgChangesHighLight("../../../axon1.svg", new[] { xmlObject });

highLighter.SaveFile("../../../axon-colored.svg");