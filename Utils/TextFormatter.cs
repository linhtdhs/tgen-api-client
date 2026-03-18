using System;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace tgenapiclient.Utils;

public static class TextFormatter
{
    public static string FormatBody(string text, string indentString = "  ")
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        var trimmed = text.Trim();
        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
        {
            var parsedJson = JsonDocument.Parse(trimmed);
            var options = new JsonSerializerOptions { WriteIndented = true };
            
            // Configure .NET 9/10 indentation
            if (indentString == "\t")
            {
                options.IndentCharacter = '\t';
                options.IndentSize = 1;
            }
            else
            {
                options.IndentCharacter = ' ';
                options.IndentSize = indentString.Length > 0 ? indentString.Length : 2;
            }
            
            return JsonSerializer.Serialize(parsedJson, options);
        }
        else if (trimmed.StartsWith("<"))
        {
            var parsedXml = XDocument.Parse(trimmed);
            var settings = new System.Xml.XmlWriterSettings
            {
                Indent = true,
                IndentChars = indentString,
                OmitXmlDeclaration = true
            };
            
            using var sw = new System.IO.StringWriter();
            using var xw = System.Xml.XmlWriter.Create(sw, settings);
            parsedXml.Save(xw);
            return sw.ToString();
        }
        return text;
    }

    public static string FormatHeaders(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        var lines = text.Split(new[] { '\n' });
        var formattedHeaders = new StringBuilder();
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(line)) continue;

            var index = line.IndexOf(':');
            if (index > 0)
            {
                var key = line.Substring(0, index).Trim();
                var value = line.Substring(index + 1).Trim();
                formattedHeaders.AppendLine($"{key}: {value}");
            }
            else
            {
                throw new HeaderFormatException($"Invalid header format. Expected 'Key: Value'.", i, line.Length);
            }
        }
        return formattedHeaders.ToString().TrimEnd();
    }
}
