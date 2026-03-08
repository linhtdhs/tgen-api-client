using System;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace tgenapiclient.Utils;

public static class TextFormatter
{
    public static string FormatBody(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        var trimmed = text.Trim();
        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
        {
            var parsedJson = JsonDocument.Parse(trimmed);
            return JsonSerializer.Serialize(parsedJson, new JsonSerializerOptions { WriteIndented = true });
        }
        else if (trimmed.StartsWith("<"))
        {
            var parsedXml = XDocument.Parse(trimmed);
            return parsedXml.ToString();
        }
        return text;
    }

    public static string FormatHeaders(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var formattedHeaders = new StringBuilder();
        foreach (var line in lines)
        {
            var index = line.IndexOf(':');
            if (index > 0)
            {
                var key = line.Substring(0, index).Trim();
                var value = line.Substring(index + 1).Trim();
                formattedHeaders.AppendLine($"{key}: {value}");
            }
            else
            {
                formattedHeaders.AppendLine(line.Trim());
            }
        }
        return formattedHeaders.ToString().TrimEnd();
    }
}
