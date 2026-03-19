using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using tgenapiclient.Constants;
using tgenapiclient.Models;

namespace tgenapiclient.Services;

public class HistoryService
{
    public ObservableCollection<HistoryItem> History { get; } = new();

    public HistoryService()
    {
        LoadHistory();
    }

    public void AddEntry(HistoryItem item)
    {
        // Insert at the top for newest first
        History.Insert(0, item);
        SaveHistory();
    }

    public void ClearHistory()
    {
        History.Clear();
        SaveHistory();
    }

    public void RemoveEntry(HistoryItem item)
    {
        History.Remove(item);
        SaveHistory();
    }

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
