using System.Xml;

namespace PumlExporter;

public class FileInput : IFileInput
{
    private readonly string _newFileAsText;
    public XmlReader OldDataReader { get; }
    public XmlReader NewDataReader  => new XmlTextReader(new StringReader(_newFileAsText));

    public FileInput(string lastFilePath, string newFilePath)
    {
        _newFileAsText = File.ReadAllText(newFilePath).Replace("<text fill=\"#000000\"",
                "<text fill=\"#383838\"").Replace("font-size=\"14\"", "font-size=\"12\"");

        OldDataReader = new XmlTextReader(File.Open(lastFilePath, FileMode.Open));
    }
}