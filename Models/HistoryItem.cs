using System;
using tgenapiclient.Constants;

namespace tgenapiclient.Models;

public class HistoryItem
{
    /// <summary>
    /// Unique identifier for the history item.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Timestamp when the request was made.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    // Request details
    /// <summary>
    /// HTTP method used for the request.
    /// </summary>
    public string Method { get; set; } = AppConstants.DefaultMethod;

    /// <summary>
    /// URL of the request.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Raw request headers.
    /// </summary>
    public string RequestHeaders { get; set; } = string.Empty;

    /// <summary>
    /// Raw request body.
    /// </summary>
    public string RequestBody { get; set; } = string.Empty;
    
    // Response details
    /// <summary>
    /// HTTP status code of the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Reason phrase associated with the response status code.
    /// </summary>
    public string ReasonPhrase { get; set; } = string.Empty;

    /// <summary>
    /// Time taken to process the request and receive the response in ms.
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// Size of the response body in bytes.
    /// </summary>
    public int SizeBytes { get; set; }

    /// <summary>
    /// Formatted response headers.
    /// </summary>
    public string ResponseHeaders { get; set; } = string.Empty;

    /// <summary>
    /// Raw response body.
    /// </summary>
    public string ResponseBody { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the HTTP request was successful.
    /// </summary>
    public bool IsSuccess { get; set; }
    
    // UI Helper properties
    /// <summary>
    /// Formatted timestamp for UI display.
    /// </summary>
    public string DisplayTime => Timestamp.ToString(AppConstants.HistoryTimeFormat);

    /// <summary>
    /// Summary of the request (method and URL) for UI display.
    /// </summary>
    public string DisplaySummary => $"{Method} {Url}";
}
