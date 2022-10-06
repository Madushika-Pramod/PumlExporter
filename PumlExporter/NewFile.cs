using System.Xml;

namespace PumlExporter;

public class NewFile : SvgFile
{
    private NewFile(Builder builder) : base(builder)
    {
    }
    

    public new class Builder : SvgFile.Builder
    {
        private string _body;
        private XmlTextReader DataReader { get; set; }

        public Builder(RelativeFilePath filePath) : this(File.ReadAllText(filePath.Path))
        {
        }

        public Builder(string body)
        {
            _body = body;
            DataReader = new XmlTextReader(new StringReader(body));
        }

        


        public Builder Update(string textColor = "#383838", string fontSize = "12")
        {
            // if (_body == null)
            // {
            //     throw new Exception("File update error: context doesn't provided");
            // }

            _body = _body.Replace("<text fill=\"#000000\"", $"<text fill=\"{textColor}\"")
                .Replace("font-size=\"14\"", $"font-size=\"{fontSize}\"");

            DataReader = new XmlTextReader(new StringReader(_body));
            return this;
        }

        public NewFile Build()
        {
            XmlDocument.Load(DataReader);
            return new NewFile(this);
        }
    }
}