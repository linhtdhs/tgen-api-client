namespace TGenApiClient.Core.Constants;

public static class AppConstants
{
    /// <summary>
    /// File name for the SQLite database tracking request history.
    /// </summary>
    public const string DatabaseFileName = "tgenapi_history.db";

    /// <summary>
    /// SQLite connection string format.
    /// </summary>
    public const string DatabaseConnectionStringFormat = "Data Source={0}";

    /// <summary>
    /// Standard MIME type for JSON requests.
    /// </summary>
    public const string JsonMediaType = "application/json";

    /// <summary>
    /// Maximum limit for history records stored in memory and tracked before purging.
    /// </summary>
    public const int MaxHistoryLimit = 100;

    /// <summary>
    /// Default name for the base operational environment profile.
    /// </summary>
    public const string DefaultEnvironmentName = "Default";

    /// <summary>
    /// The default variable key used to resolve base URL endpoints.
    /// </summary>
    public const string DefaultBaseUrlVariable = "baseUrl";

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
