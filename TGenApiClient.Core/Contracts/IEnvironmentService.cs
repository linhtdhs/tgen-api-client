using System.Collections.ObjectModel;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Contracts;

/// <summary>
/// Defines a service for managing environments and variable interpolations.
/// </summary>
public interface IEnvironmentService
{
    /// <summary>
    /// Observable list of configured environments.
    /// </summary>
    ObservableCollection<AppEnvironment> Environments { get; }

    /// <summary>
    /// The currently selected environment context.
    /// </summary>
    AppEnvironment? ActiveEnvironment { get; set; }

    /// <summary>
    /// Substitutes standard variable blocks in strings for the environment value.
    /// </summary>
    /// <param name="input">The raw text block.</param>
    /// <returns>The interpolated string output.</returns>
    string ReplaceVariables(string input);
}
