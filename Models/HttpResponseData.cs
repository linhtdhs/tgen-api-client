namespace tgenapiclient.Models;

public class HttpResponseData
{
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
    public int SizeBytes { get; set; }
    public string Body { get; set; } = string.Empty;
    public string Headers { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
}
