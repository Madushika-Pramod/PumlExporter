﻿using PumlExporter;

var oldXml = SvgFile.GetXml(new FilePath("../../../axon1.svg"));
var newXml = SvgFile.GetXml(new FilePath("../../../axon2.svg"));
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
var xmlObject = new XmlObject(options, ObjectType.Element);

// set new xml for selector
var highLighter = new HighLightXml.Builder()
    .AddBackgroundNodes(xmlObject.ObjectType, "rect")
    .SetSelector(newXml)
    .Build();

highLighter.SvgGlobalHighLight("text",
    new SvgAttribute("fill", "#383838"),
    new SvgAttribute("font-size", "12"));

highLighter.SvgChangesHighLight(oldXml, new[] { xmlObject });

newXml.Save(new FilePath("../../../axon-colored.svg").Path);