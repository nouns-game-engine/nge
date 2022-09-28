using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nouns.Core.Web3;

public static class DataUri
{
    public const string Base64 = ";base64";
    public const string ApplicationJsonBase64 = $"data:application/json{Base64},";
    
    public class Format
    {
        public bool IsBase64 { get; set; }
        public string? Extension { get; set; }
        public byte[]? Data { get; set; }
        public Vector2 Size { get; set; }
    }

    public static bool TryParseImage(string uri, out Format format)
    {
        format = new Format();
        var match = Regex.Match(uri, @"data:image/(?<type>.+?),(?<data>.+)");
        if (!match.Success)
            return false;

        var imageType = match.Groups["type"].Value.ToLowerInvariant();
        var data = match.Groups["data"].Value;

        switch (imageType)
        {
            case "svg+xml;base64":
                format.Extension = "svg";
                format.IsBase64 = true;
                break;
            case "png":
                format.Extension = "png";
                break;
            case "png;base64":
                format.Extension = "png";
                format.IsBase64 = true;
                break;
            case "bmp":
                format.Extension = "bmp";
                break;
            case "bmp;base64":
                format.Extension = "bmp";
                format.IsBase64 = true;
                break;
            case "gif":
                format.Extension = "gif";
                break;
            case "gif;base64":
                format.Extension = "gif";
                format.IsBase64 = true;
                break;
            case "svg+xml":
                format.Extension = "svg";
                break;
            default:
                format.Extension = imageType;
                break;
        }

        format.Data = format.IsBase64 ? Convert.FromBase64String(data) : Encoding.UTF8.GetBytes(data);
        return true;
    }

    public static bool TryParseApplication(string uri, out Format format)
    {
        format = new Format();
        var match = Regex.Match(uri, @"data:application/(?<type>.+?),(?<data>.+)");
        if (!match.Success)
            return false;

        var applicationType = match.Groups["type"].Value.ToLowerInvariant();
        var data = match.Groups["data"].Value;

        switch (applicationType)
        {
            case "json;base64":
                format.Extension = "json";
                format.IsBase64 = true;
                break;
            case "json":
                format.Extension = "json";
                break;
            default:
                format.Extension = applicationType;
                break;
        }

        format.Data = format.IsBase64 ? Convert.FromBase64String(data) : Encoding.UTF8.GetBytes(data);
        return true;

    }
}