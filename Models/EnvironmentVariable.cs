namespace tgenapiclient.Models;

public class EnvironmentVariable
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;

    public EnvironmentVariable() { }

    public EnvironmentVariable(string key, string value, bool isEnabled = true)
    {
        Key = key;
        Value = value;
        IsEnabled = isEnabled;
    }
}
