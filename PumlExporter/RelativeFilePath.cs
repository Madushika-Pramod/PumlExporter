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

    // private string PathExtractor()
    // {
    //     if (_filePath.EndsWith(".svg"))
    //     {
    //         // var path = Regex.Replace(_filePath, ".*\\/(?=[^\\/]\\w*\\.svg)", "../../../");
    //         var path = _filePath.LastIndexOf('/');
    //         return path == -1
    //             ? $"../../../{_filePath}"
    //             : $"../../..{_filePath[_filePath.LastIndexOf('/')..]}";
    //     }
    //     throw new Exception("invalid file path");
    // }
}