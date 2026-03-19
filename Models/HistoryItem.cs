using System;
using tgenapiclient.Constants;

namespace tgenapiclient.Models;

public class HistoryItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    // Request details
    public string Method { get; set; } = AppConstants.DefaultMethod;
    public string Url { get; set; } = string.Empty;
    public string RequestHeaders { get; set; } = string.Empty;
    public string RequestBody { get; set; } = string.Empty;
    
    // Response details
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
    public int SizeBytes { get; set; }
    public string ResponseHeaders { get; set; } = string.Empty;
    public string ResponseBody { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    
    // UI Helper properties
    public string DisplayTime => Timestamp.ToString(AppConstants.HistoryTimeFormat);
    public string DisplaySummary => $"{Method} {Url}";
}
