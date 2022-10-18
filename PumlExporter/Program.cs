using PumlExporter;

var oldSvg = File.ReadAllText("../../../axon1.svg");
var newSvg = File.ReadAllText("../../../axon2.svg");

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

highLighter.SvgGlobalHighLight("text",newSvg,
    new SvgAttribute("fill", "#383838"),
    new SvgAttribute("font-size", "12"));

highLighter.SvgChangesHighLight(oldSvg, new[] { xmlObject });

var highLightedFile = highLighter.GetHighLightedFile();
highLightedFile?.Save("../../../axon-colored.svg");