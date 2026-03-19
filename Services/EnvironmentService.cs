using System.Collections.ObjectModel;
using System.Linq;
using tgenapiclient.Constants;
using tgenapiclient.Models;

namespace tgenapiclient.Services;

public class EnvironmentService
{
    public ObservableCollection<AppEnvironment> Environments { get; } = new();

    public AppEnvironment? ActiveEnvironment { get; set; }

    public EnvironmentService()
    {
        // Add a default environment
        var defaultEnv = new AppEnvironment("Default");
        defaultEnv.Variables.Add(new EnvironmentVariable("baseUrl", AppConstants.DefaultBaseUrl));
        Environments.Add(defaultEnv);
        ActiveEnvironment = defaultEnv;
    }

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
