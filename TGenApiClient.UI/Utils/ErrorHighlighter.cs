using System;
using Avalonia.Controls;

namespace TGenApiClient.UI.Utils;

public static class ErrorHighlighter
{
    public static void Highlight(TextBox textBox, string text, long? lineIndex, long? charPosition)
    {
        if (lineIndex == null || charPosition == null) return;
        
        var lines = text.Split('\n');
        long totalChars = 0;
        
        for (int i = 0; i < lineIndex.Value && i < lines.Length; i++)
        {
            totalChars += lines[i].Length + 1; // +1 for newline character
        }
        
        totalChars += charPosition.Value;

        if (totalChars >= 0 && totalChars <= text.Length)
        {
            // Focus and select the problematic character to highlight where to fix
            textBox.SelectionStart = (int)totalChars;
            textBox.SelectionEnd = (int)Math.Min(totalChars + 1, text.Length);
            textBox.Focus();
        }
    }
}
