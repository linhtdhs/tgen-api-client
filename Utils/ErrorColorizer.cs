using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace tgenapiclient.Utils;

public class ErrorColorizer : DocumentColorizingTransformer
{
    private int _errorOffset = -1;
    private int _errorLength = 0;

    public void SetError(int offset, int length)
    {
        _errorOffset = offset;
        _errorLength = length;
    }

    public void ClearError()
    {
        _errorOffset = -1;
        _errorLength = 0;
    }

    protected override void ColorizeLine(DocumentLine line)
    {
        if (_errorOffset < 0 || _errorLength <= 0) return;
        
        int lineStart = line.Offset;
        int lineEnd = line.Offset + line.Length;

        if (_errorOffset >= lineStart && _errorOffset <= lineEnd)
        {
            int start = _errorOffset;
            int end = _errorOffset + _errorLength;
            if (end > lineEnd) end = lineEnd;

            ChangeLinePart(
                start,
                end,
                element => {
                    element.TextRunProperties.SetForegroundBrush(Brushes.Red);
                    element.TextRunProperties.SetTextDecorations(TextDecorations.Underline);
                });
        }
    }
}
