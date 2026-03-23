using System.Collections.ObjectModel;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Contracts;

public interface IEnvironmentService
{
    ObservableCollection<AppEnvironment> Environments { get; }
    AppEnvironment? ActiveEnvironment { get; set; }
    string ReplaceVariables(string input);
}
