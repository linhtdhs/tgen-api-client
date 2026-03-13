using System;
using System.Net.Http;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaEdit;
using tgenapiclient.Models;
using tgenapiclient.Services;
using tgenapiclient.Utils;

namespace tgenapiclient;

public partial class MainWindow : Window
{
    private readonly HttpService _httpService = new HttpService();
    private readonly EnvironmentService _envService = new EnvironmentService();

    public MainWindow()
    {
        InitializeComponent();
        
        EnvComboBox.ItemsSource = _envService.Environments;
        EnvComboBox.SelectedItem = _envService.ActiveEnvironment;

        var colorizer = new EnvironmentColorizer();
        RequestHeadersEditor.TextArea.TextView.LineTransformers.Add(colorizer);
        RequestBodyEditor.TextArea.TextView.LineTransformers.Add(colorizer);
        
        RequestHeadersEditor.LostFocus += Editor_LostFocus;
        RequestBodyEditor.LostFocus += Editor_LostFocus;
    }

    private void EnvComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (EnvComboBox.SelectedItem is AppEnvironment env)
        {
            _envService.ActiveEnvironment = env;
        }
    }

    private async void ManageEnvButton_Click(object? sender, RoutedEventArgs e)
    {
        var envWindow = new EnvironmentWindow(_envService);
        await envWindow.ShowDialog(this);
    }

    private async void SendButton_Click(object? sender, RoutedEventArgs e)
    {
        SendButton.IsEnabled = false;
        StatusTextBlock.Text = "Sending...";
        TimeTextBlock.Text = "--- ms";
        ResponseBodyEditor.Text = string.Empty;
        ResponseHeadersEditor.Text = string.Empty;

        var url = _envService.ReplaceVariables(UrlTextBox.Text ?? string.Empty);
        var methodStr = (MethodComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "GET";
        var method = new HttpMethod(methodStr);
        var headersText = _envService.ReplaceVariables(RequestHeadersEditor.Text ?? string.Empty);
        var bodyText = _envService.ReplaceVariables(RequestBodyEditor.Text ?? string.Empty);

        try
        {
            var response = await _httpService.SendRequestAsync(method, url, headersText, bodyText);

            // Update UI
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBlock.Text = $"{response.StatusCode} {response.ReasonPhrase}";
                if (response.IsSuccess)
                    StatusTextBlock.Foreground = Avalonia.Media.Brushes.LightGreen;
                else
                    StatusTextBlock.Foreground = Avalonia.Media.Brushes.LightCoral;

                TimeTextBlock.Text = $"{response.ElapsedMilliseconds} ms";
                SizeTextBlock.Text = $"{response.SizeBytes} B";

                ResponseBodyEditor.Text = response.Body;
                ResponseHeadersEditor.Text = response.Headers;
            });
        }
        catch (Exception ex)
        {
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBlock.Text = "Error";
                StatusTextBlock.Foreground = Avalonia.Media.Brushes.Red;
                ResponseBodyEditor.Text = ex.ToString();
            });
        }
        finally
        {
            Dispatcher.UIThread.Post(() =>
            {
                SendButton.IsEnabled = true;
            });
        }
    }

    private void Editor_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextEditor editor)
        {
            FormatEditorContent(editor);
        }
    }

    private void FormatEditorContent(TextEditor editor)
    {
        var text = editor.Text;
        if (string.IsNullOrWhiteSpace(text))
        {
            DataValidationErrors.SetErrors(editor, null);
            return;
        }

        try
        {
            if (editor == RequestBodyEditor)
            {
                editor.Text = TextFormatter.FormatBody(text);
            }
            else if (editor == RequestHeadersEditor)
            {
                editor.Text = TextFormatter.FormatHeaders(text);
            }
            
            // Clear any previous errors on success
            DataValidationErrors.SetErrors(editor, null);
        }
        catch (JsonException ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
            // ErrorHighlighter needs update for TextEditor or remove for now
        }
        catch (System.Xml.XmlException ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
            // ErrorHighlighter needs update for TextEditor or remove for now
        }
        catch (Exception ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
        }
    }
}
