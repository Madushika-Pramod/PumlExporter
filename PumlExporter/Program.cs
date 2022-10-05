using PumlExporter;

var file = new PumalExporter("../../../axon1.svg",
    "../../../axon2.svg");
// file.ExportFile("../../../axon-colored.svg", new Elements("#0f00f0", "#C000CE"));
file.ExportFile("../../../axon-colored.svg");