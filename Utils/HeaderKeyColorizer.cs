using System;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace tgenapiclient.Utils;

public class HeaderKeyColorizer : DocumentColorizingTransformer
{
    protected override void ColorizeLine(DocumentLine line)
    {
        var text = CurrentContext.Document.GetText(line);
        var index = text.IndexOf(':');

        if (index > 0)
        {
            var startOffset = line.Offset;
            var endOffset = line.Offset + index;

            ChangeLinePart(
                startOffset,
                endOffset,
                element => {
                    element.TextRunProperties.SetTypeface(new Typeface(element.TextRunProperties.Typeface.FontFamily, FontStyle.Italic, element.TextRunProperties.Typeface.Weight));
                });
        }
    }
}
