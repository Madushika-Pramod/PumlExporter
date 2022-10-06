using System.Xml;

namespace PumlExporter;

public class OldFile
{
    public readonly XmlDocument XmlDocument;

    private OldFile(Builder builder)
    {
        XmlDocument = builder.XmlDocument;
    }

    public class Builder
    {
        
        internal readonly XmlDocument XmlDocument = new();
        // private XmlReader DataReader => new XmlTextReader(File.Open(_filePath, FileMode.Open));
        private readonly XmlReader _dataReader;

        public Builder(RelativeFilePath filePath):this(new XmlTextReader(File.Open(filePath.Path, FileMode.Open)))
        {
            // _filePath = filePath;
        }
        public Builder(string body):this(new XmlTextReader(new StringReader(body)))
        {
        }

        public Builder(XmlReader xmlReader)
        {
            _dataReader = xmlReader;
        }

        public OldFile Build()
        {
            XmlDocument.Load(_dataReader);
            return new(this);
        }
    }
}