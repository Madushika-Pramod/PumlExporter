using System.Threading.Channels;
using PumlExporter;

var _ = new PumalExporter( new ColorOptions("#000000","#C5CECE") ,"../../../axon1.svg",
    "../../../axon2.svg","../../../axon-colored.svg");