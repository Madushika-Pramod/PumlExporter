using System.Text.RegularExpressions;

namespace PumlExporter;

public class RelativeFilePath
{
    private readonly string _filePath;

    public string Path => _filePath.EndsWith(".svg")
        ? _filePath.LastIndexOf('/') != -1
            ? $"../../..{_filePath[_filePath.LastIndexOf('/')..]}"
            : $"../../../{_filePath}"
        : throw new Exception("invalid file path");

    public RelativeFilePath(string filePath)
    {
        _filePath = filePath;
    }
}