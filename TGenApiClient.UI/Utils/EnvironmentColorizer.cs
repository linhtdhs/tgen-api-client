using System;
using System.Text.RegularExpressions;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace TGenApiClient.UI.Utils;

public class EnvironmentColorizer : DocumentColorizingTransformer
{
    private readonly Regex _envRegex = new Regex(@"\{\{[^{}]+\}\}", RegexOptions.Compiled);
    private readonly IBrush _highlightBrush = Brushes.Orange;

    protected override void ColorizeLine(DocumentLine line)
    {
        var text = CurrentContext.Document.GetText(line);
        var matches = _envRegex.Matches(text);

        foreach (Match match in matches)
        {
            var startOffset = line.Offset + match.Index;
            var endOffset = startOffset + match.Length;

            ChangeLinePart(
                startOffset,
                endOffset,
                element => {
                    element.TextRunProperties.SetForegroundBrush(_highlightBrush);
                });
        }
    }
}
