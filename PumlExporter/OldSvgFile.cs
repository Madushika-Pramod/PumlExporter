using System.Xml;

namespace PumlExporter;

public class OldSvgFile: SvgFile
{
    private OldSvgFile(Builder builder): base(builder)
    {
    }
    private static Dictionary<string, XmlNodeList> OldFileNodes(XmlNodeList oldNodeList)
    {
        var oldFileNodes = new Dictionary<string, XmlNodeList>();
        foreach (XmlElement node in oldNodeList)
        {
            var key = node.GetAttribute("id");

            oldFileNodes.Add(key, node.ChildNodes);
        }

        return oldFileNodes;
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


        public new Builder SetElements(params ObjectType[] types) // what's the difference override and new
        {
            XmlDocument.Load(DataReader);
            return (Builder)base.SetElements(types);
        }

        public OldSvgFile Build()
        {
            return new OldSvgFile(this);
        }
    }
}