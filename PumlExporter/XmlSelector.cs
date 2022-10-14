using System.Xml;
namespace PumlExporter;
public static class XmlSelector
{
    // private static string Objects(string obj) => $"//s:g[1]/s:g[starts-with(@id,'{obj}')]";
    // private static string Nodes(string nodeName) => "descendant::s:" + nodeName;

    public static XmlNodeList GetObjectNodeList(XmlDocument xmlDocument, XmlNamespaceManager manager,
        string objectType = "elem_")
    {
        return xmlDocument.SelectNodes( $"//s:g[1]/s:g[starts-with(@id,'{objectType}')]", manager) ??
               throw new Exception("XmlDocument doesn't have element nodes");
    }

    public static XmlNodeList GetGlobalNodeList(XmlDocument xmlDocument, XmlNamespaceManager manager,
        string nodeName = "text")
    {
        return xmlDocument.SelectNodes("descendant::s:" + nodeName, manager) ??
               throw new Exception("XmlDocument doesn't have element nodes");
    }
}