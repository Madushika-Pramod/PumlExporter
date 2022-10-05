using System.Xml;

namespace PumlExporter;

public interface IFileInput
{
    XmlReader OldDataReader { get; }
    XmlReader NewDataReader { get; }
}