using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace tgenapiclient.Models;

public class AppEnvironment : INotifyPropertyChanged
{
    private string _name = string.Empty;

    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name 
    { 
        get => _name; 
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ObservableCollection<EnvironmentVariable> Variables { get; set; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public AppEnvironment() { }

    public AppEnvironment(string name)
    {
        Name = name;
    }
}
