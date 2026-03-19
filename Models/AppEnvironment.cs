using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace tgenapiclient.Models;

public class AppEnvironment : INotifyPropertyChanged
{
    private string _name = string.Empty;

    /// <summary>
    /// Unique identifier for the environment.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Display name of the environment.
    /// </summary>
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
    
    /// <summary>
    /// Collection of variables associated with this environment.
    /// </summary>
    public ObservableCollection<EnvironmentVariable> Variables { get; set; } = new();

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Invokes the PropertyChanged event for a given property.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed. If not provided, the caller's member name is used.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Initializes a new, empty instance of the <see cref="AppEnvironment"/> class.
    /// </summary>
    public AppEnvironment() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppEnvironment"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name to assign to the environment.</param>
    public AppEnvironment(string name)
    {
        Name = name;
    }
}
