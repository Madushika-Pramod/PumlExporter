using PumlExporter;

var oldFile = new OldFile.Builder(new RelativeFilePath("axon1.svg")).Build();
var newFile = new NewFile.Builder(new RelativeFilePath("axon2.svg")).Update().Build();
var decorator = new PumalDecorator();
decorator.ExportFile(newFile, oldFile, new RelativeFilePath("axon-colored.svg"));