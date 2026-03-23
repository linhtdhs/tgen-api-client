using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using TGenApiClient.Core.Constants;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Services;

public class HistoryService
{
    /// <summary>
    /// Collection of history items observable by the UI.
    /// </summary>
    public ObservableCollection<HistoryItem> History { get; } = new();

    /// <summary>
    /// Initializes a new instance of HistoryService and loads history from the file.
    /// </summary>
    public HistoryService()
    {
        LoadHistory();
    }

    /// <summary>
    /// Adds a new entry to the history collection and saves to persistent storage.
    /// </summary>
    /// <param name="item">The history item to add.</param>
    public void AddEntry(HistoryItem item)
    {
        // Insert at the top for newest first
        History.Insert(0, item);
        SaveHistory();
    }

    /// <summary>
    /// Clears all entries from the history collection and persistent storage.
    /// </summary>
    public void ClearHistory()
    {
        History.Clear();
        SaveHistory();
    }

    /// <summary>
    /// Removes a specific entry from the history collection.
    /// </summary>
    /// <param name="item">The history item to remove.</param>
    public void RemoveEntry(HistoryItem item)
    {
        History.Remove(item);
        SaveHistory();
    }

    /// <summary>
    /// Saves the current history collection to a local JSON file.
    /// </summary>
    private void SaveHistory()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(History, options);
            File.WriteAllText(AppConstants.HistoryFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save history: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads the history collection from the local JSON file if it exists.
    /// </summary>
    private void LoadHistory()
    {
        if (File.Exists(AppConstants.HistoryFilePath))
        {
            try
            {
                var json = File.ReadAllText(AppConstants.HistoryFilePath);
                var items = JsonSerializer.Deserialize<ObservableCollection<HistoryItem>>(json);
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        History.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load history: {ex.Message}");
            }
        }
    }
}
