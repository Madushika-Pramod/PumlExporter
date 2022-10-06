using System.Xml;

namespace PumlExporter;

public abstract class SvgFile
{
    public XmlDocument XmlDocument { get; }

    protected SvgFile(Builder builder)
    {
        XmlDocument = builder.XmlDocument;
    }

    public XmlNodeList? GetNodeLists(PumlType type, XmlNamespaceManager nameSpaceManager)
    {
        return XmlDocument.SelectNodes($"//s:g[1]/s:g[starts-with(@id,'{type}')]",
            nameSpaceManager);
    }

    public abstract class Builder
    {
        public readonly XmlDocument XmlDocument = new(); // todo <==
    }
}