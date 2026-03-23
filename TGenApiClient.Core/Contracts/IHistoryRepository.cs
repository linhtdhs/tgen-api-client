using System.Collections.ObjectModel;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Contracts;

public interface IHistoryRepository
{
    ObservableCollection<HistoryItem> History { get; }
    void AddEntry(HistoryItem item);
    void ClearHistory();
}
