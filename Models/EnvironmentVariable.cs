namespace tgenapiclient.Models;

public class EnvironmentVariable
{
    /// <summary>
    /// The key or name of the environment variable.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The value assigned to the environment variable.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the environment variable is currently enabled for replacement.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Initializes a new, empty instance of the <see cref="EnvironmentVariable"/> class.
    /// </summary>
    public EnvironmentVariable() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentVariable"/> class with the specified key, value, and enabled state.
    /// </summary>
    /// <param name="key">The defined variable key.</param>
    /// <param name="value">The value to substitute.</param>
    /// <param name="isEnabled">Whether this variable is active.</param>
    public EnvironmentVariable(string key, string value, bool isEnabled = true)
    {
        Key = key;
        Value = value;
        IsEnabled = isEnabled;
    }
}
