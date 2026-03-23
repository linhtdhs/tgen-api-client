using System.Collections.ObjectModel;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Contracts;

/// <summary>
/// Defines a repository for managing API request and response history.
/// </summary>
public interface IHistoryRepository
{
    /// <summary>
    /// Gets the observable collection of history items for UI binding.
    /// </summary>
    ObservableCollection<HistoryItem> History { get; }

    /// <summary>
    /// Adds a new historical request/response entry to the repository and persistent storage.
    /// </summary>
    /// <param name="item">The composed <see cref="HistoryItem"/> to track.</param>
    void AddEntry(HistoryItem item);

    /// <summary>
    /// Clears all historical entries from both memory and persistent storage.
    /// </summary>
    void ClearHistory();
}
