using System;

namespace TGenApiClient.UI.Utils;

public class HeaderFormatException : Exception
{
    public int LineNumber { get; }
    public int CharPosition { get; }

    public HeaderFormatException(string message, int lineNumber, int charPosition) : base(message)
    {
        LineNumber = lineNumber;
        CharPosition = charPosition;
    }
}
