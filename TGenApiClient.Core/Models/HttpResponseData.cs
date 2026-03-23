namespace TGenApiClient.Core.Models;

public class HttpResponseData
{
    /// <summary>
    /// HTTP status code returned by the server.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Reason phrase associated with the status code.
    /// </summary>
    public string ReasonPhrase { get; set; } = string.Empty;

    /// <summary>
    /// Time taken to receive the response in milliseconds.
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// Size of the response body in bytes.
    /// </summary>
    public int SizeBytes { get; set; }

    /// <summary>
    /// Raw string content of the response body.
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Formatted string containing the response headers.
    /// </summary>
    public string Headers { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the HTTP response was successful.
    /// </summary>
    public bool IsSuccess { get; set; }
}
