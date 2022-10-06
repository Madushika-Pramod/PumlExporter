using System.Xml;

namespace PumlExporter;

public class NewFile
{
    public readonly XmlDocument XmlDocument;

    private NewFile(Builder builder)
    {
        XmlDocument = builder.XmlDocument;
    }

    public class Builder
    {
        internal readonly XmlDocument XmlDocument = new();
        private string? _body;
        private XmlReader _dataReader;

        public Builder(RelativeFilePath filePath) : this(File.ReadAllText(filePath.Path))
        {
        }

        public Builder(string body)
        {
            _body = body;
            _dataReader = new XmlTextReader(new StringReader(body));
        }


        public Builder(XmlTextReader xmlTextReader)
        {
            _dataReader = xmlTextReader;
        }


        public Builder Update(string textColor = "#383838", string fontSize = "12")
        {
            if (_body == null)
            {
                throw new Exception("File update error: context doesn't provided");
            }

            _body = _body.Replace("<text fill=\"#000000\"", $"<text fill=\"{textColor}\"")
                .Replace("font-size=\"14\"", $"font-size=\"{fontSize}\"");

            _dataReader = new XmlTextReader(new StringReader(_body));


            return this;
        }

        public NewFile Build()
        {
            XmlDocument.Load(_dataReader);
            return new NewFile(this);
        }
    }
}