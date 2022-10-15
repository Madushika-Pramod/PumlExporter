using System.Xml;
using PumlExporter;
using Attribute = PumlExporter.Attribute;

var oldXml = SvgFile.GetXml(new FilePath("../../../axon1.svg"));
var newXml = SvgFile.GetXml(new FilePath("../../../axon2.svg"));
var nameSpaceManager = new XmlNamespaceManager(oldXml.NameTable);
nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

var options = new Dictionary<string, Attribute[]>
{
    {
        "text",
        new[] { new Attribute("fill", "#000000"), new Attribute("font-size", "14") }
    },
    {
        "rect", new[] { new Attribute("fill", "#C5CECE") }
    }
};
var xmlObject = new XmlObject(options,"elem_");
var highLighter = new HighLightXml();
highLighter.BackgroundNodes.Add(xmlObject.ObjectType,"rect");

highLighter.SvgGlobalHighLight(
    XmlSelector.GetGlobalNodeList(newXml, nameSpaceManager, "text"),
    new Attribute("fill", "#383838"),
    new Attribute("font-size", "12"));

highLighter.SvgChangesHighLight(
    XmlSelector.GetObjectNodeList(newXml, nameSpaceManager,xmlObject.ObjectType),
    XmlSelector.GetObjectNodeList(oldXml, nameSpaceManager,xmlObject.ObjectType),
    new[] { xmlObject });

newXml.Save(new FilePath("../../../axon-colored.svg").Path);