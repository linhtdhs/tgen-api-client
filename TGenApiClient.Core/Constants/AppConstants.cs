namespace TGenApiClient.Core.Constants;

public static class AppConstants
{
    /// <summary>
    /// Path to the local JSON file where request history is saved.
    /// </summary>
    public const string HistoryFilePath = "history.json";

    /// <summary>
    /// Default HTTP method used when initializing a new request.
    /// </summary>
    public const string DefaultMethod = "GET";

    /// <summary>
    /// Default URL used as a placeholder in the request editor.
    /// </summary>
    public const string DefaultUrl = "{{baseUrl}}/posts/1";

    /// <summary>
    /// Default base URL used in the environment variables.
    /// </summary>
    public const string DefaultBaseUrl = "https://jsonplaceholder.typicode.com";

    /// <summary>
    /// Format string used for displaying the timestamp in history entries.
    /// </summary>
    public const string HistoryTimeFormat = "dd/MM HH:mm:ss";
}
