using System;
using System.Collections.ObjectModel;
using System.Linq;
using TGenApiClient.Core.Contracts;
using TGenApiClient.Core.Data;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Services;

public class HistoryService : IHistoryRepository
{
    private readonly HistoryDbContext _dbContext;
    public ObservableCollection<HistoryItem> History { get; } = new();

    public HistoryService()
    {
        _dbContext = new HistoryDbContext();
        LoadHistory();
    }

    public void AddEntry(HistoryItem item)
    {
        History.Insert(0, item);
        
        if (History.Count > 100)
        {
            var itemToRemove = History.Last();
            History.RemoveAt(History.Count - 1);
            try {
                _dbContext.HistoryItems.Remove(itemToRemove);
            } catch { /* Ignored if not found */ }
        }

        try 
        {
            _dbContext.HistoryItems.Add(item);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save history entry to DB: {ex.Message}");
        }
    }

    public void ClearHistory()
    {
        History.Clear();
        try 
        {
            _dbContext.HistoryItems.RemoveRange(_dbContext.HistoryItems);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to clear history from DB: {ex.Message}");
        }
    }

    private void LoadHistory()
    {
        try
        {
            var items = _dbContext.HistoryItems.OrderBy(h => h.Timestamp).ToList();
            foreach (var item in items)
            {
                History.Insert(0, item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load history from DB: {ex.Message}");
        }
    }
}
