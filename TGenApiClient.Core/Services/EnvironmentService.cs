using System.Collections.ObjectModel;
using System.Linq;
using TGenApiClient.Core.Constants;
using TGenApiClient.Core.Contracts;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Services;

public class EnvironmentService : IEnvironmentService
{
    /// <summary>
    /// Collection of all available environments.
    /// </summary>
    public ObservableCollection<AppEnvironment> Environments { get; } = new();

    /// <summary>
    /// Gets or sets the currently active environment used for variable replacement.
    /// </summary>
    public AppEnvironment? ActiveEnvironment { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentService"/> class and sets up the default environment.
    /// </summary>
    public EnvironmentService()
    {
        // Add a default environment
        var defaultEnv = new AppEnvironment(AppConstants.DefaultEnvironmentName);
        defaultEnv.Variables.Add(new EnvironmentVariable(AppConstants.DefaultBaseUrlVariable, AppConstants.DefaultBaseUrl));
        Environments.Add(defaultEnv);
        ActiveEnvironment = defaultEnv;
    }

    /// <summary>
    /// Replaces occurrences of environment variable placeholders in the input string with their corresponding values.
    /// </summary>
    /// <param name="input">The original text containing templates like {{baseUrl}}.</param>
    /// <returns>The interpolated string with values substituted, or the original string if no environment is active.</returns>
    public string ReplaceVariables(string input)
    {
        if (string.IsNullOrEmpty(input) || ActiveEnvironment == null)
            return input;

        var result = input;
        foreach (var variable in ActiveEnvironment.Variables.Where(v => v.IsEnabled && !string.IsNullOrEmpty(v.Key)))
        {
            var placeholder = "{{" + variable.Key + "}}";
            result = result.Replace(placeholder, variable.Value);
        }

        return result;
    }
}
