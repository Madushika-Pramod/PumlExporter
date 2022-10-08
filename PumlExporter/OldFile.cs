using System.Xml;

namespace PumlExporter;

public class OldFile: SvgFile
{
    private OldFile(Builder builder): base(builder)
    {
    }

    public new class Builder : SvgFile.Builder
    {
        // private XmlReader DataReader => new XmlTextReader(File.Open(_filePath, FileMode.Open));
        private XmlReader DataReader { get; }
        public Builder(RelativeFilePath filePath):this(new XmlTextReader(File.Open(filePath.Path, FileMode.Open)))
        {
            // _filePath = filePath;
        }
        public Builder(string body):this(new XmlTextReader(new StringReader(body)))
        {
        }

        public Builder(XmlReader xmlReader)
        {
            DataReader = xmlReader;
        }


        public new Builder SetElementsAndLinks(params ObjectType[] types) // what's the difference override and new
        {
            XmlDocument.Load(DataReader);
            return (Builder)base.SetElementsAndLinks(types);
        }

        public OldFile Build()
        {
            return new OldFile(this);
        }
    }
}