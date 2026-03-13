using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using tgenapiclient.Models;
using tgenapiclient.Services;

namespace tgenapiclient;

public partial class EnvironmentWindow : Window
{
    private readonly EnvironmentService _envService;

    public EnvironmentWindow(EnvironmentService envService)
    {
        InitializeComponent();
        _envService = envService;

        EnvironmentsListBox.ItemsSource = _envService.Environments;
        if (_envService.Environments.Any())
        {
            EnvironmentsListBox.SelectedItem = _envService.ActiveEnvironment ?? _envService.Environments.First();
        }
    }

    public EnvironmentWindow()
    {
        InitializeComponent();
        _envService = new EnvironmentService(); // Design-time only ideally
    }

    private void EnvironmentsListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (EnvironmentsListBox.SelectedItem is AppEnvironment selectedEnv)
        {
            VariablesGrid.IsEnabled = true;
            EnvNameTextBox.Text = selectedEnv.Name;
            VariablesItemsControl.ItemsSource = selectedEnv.Variables;
        }
        else
        {
            VariablesGrid.IsEnabled = false;
            EnvNameTextBox.Text = string.Empty;
            VariablesItemsControl.ItemsSource = null;
        }
    }

    private void AddEnvButton_Click(object? sender, RoutedEventArgs e)
    {
        var newEnv = new AppEnvironment($"New Environment {_envService.Environments.Count + 1}");
        _envService.Environments.Add(newEnv);
        EnvironmentsListBox.SelectedItem = newEnv;
    }

    private void RemoveEnvButton_Click(object? sender, RoutedEventArgs e)
    {
        if (EnvironmentsListBox.SelectedItem is AppEnvironment selectedEnv)
        {
            _envService.Environments.Remove(selectedEnv);
        }
    }

    private void EnvNameTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (EnvironmentsListBox.SelectedItem is AppEnvironment selectedEnv)
        {
            selectedEnv.Name = EnvNameTextBox.Text ?? string.Empty;
        }
    }

    private void AddVariable_Click(object? sender, RoutedEventArgs e)
    {
        if (EnvironmentsListBox.SelectedItem is AppEnvironment selectedEnv)
        {
            selectedEnv.Variables.Add(new EnvironmentVariable("newKey", "newValue"));
        }
    }

    private void RemoveVariable_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is EnvironmentVariable variable)
        {
            if (EnvironmentsListBox.SelectedItem is AppEnvironment selectedEnv)
            {
                selectedEnv.Variables.Remove(variable);
            }
        }
    }
}
