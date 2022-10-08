using System.Xml;

namespace PumlExporter;

public abstract class SvgFile
{
    public XmlDocument XmlDocument { get; }
    public XmlNodeList? Elements;

    protected SvgFile(Builder builder)
    {
        XmlDocument = builder.XmlDocument;
        Elements = builder.Elements;
    }


    public abstract class Builder
    {
        internal readonly XmlDocument XmlDocument = new(); // todo <==
        internal XmlNodeList Elements = null!;
        internal XmlNodeList Links = null!;

        protected XmlNodeList GetNodeLists(ObjectType type)
        {
            var nameSpaceManager = new XmlNamespaceManager(XmlDocument.NameTable); // creates 2 times for each file
            nameSpaceManager.AddNamespace("s", "http://www.w3.org/2000/svg");
            nameSpaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            
            return XmlDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type.ToText()}')]", nameSpaceManager) ??
                   throw new InvalidOperationException();
             
        }
        public Builder SetElements(params ObjectType[] types) // what's the difference override and new
        {
            if (types == null)
            {
                throw new Exception("Object type is not provided");
            }
            foreach (var type in types)
            {
                if (type == ObjectType.Element)
                {
                    Elements = GetNodeLists(ObjectType.Element);
                }
                else
                {
                    Links = GetNodeLists(ObjectType.Link);
                }
                
                
            }
            return this;
        }
    }
}