using PumlExporter;

var file = new PumalExporter( new ColorOptions("#000000","#C5CECE") ,"../../../axon1.svg",
    "../../../axon2.svg");
    file.ExportFile("../../../axon-colored.svg");