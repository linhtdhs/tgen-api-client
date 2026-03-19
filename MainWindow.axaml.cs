using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using tgenapiclient.Constants;
using Avalonia.Controls;
using Avalonia.Input;
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
    private readonly HistoryService _historyService = new HistoryService();
    private readonly ErrorColorizer _bodyErrorColorizer = new ErrorColorizer();
    private readonly ErrorColorizer _headersErrorColorizer = new ErrorColorizer();
    private readonly HeaderKeyColorizer _headerKeyColorizer = new HeaderKeyColorizer();

    public MainWindow()
    {
        InitializeComponent();
        
        EnvComboBox.ItemsSource = _envService.Environments;
        EnvComboBox.SelectedItem = _envService.ActiveEnvironment;
        HistoryListBox.ItemsSource = _historyService.History;

        var colorizer = new EnvironmentColorizer();
        UrlEditor.Text = AppConstants.DefaultUrl;
        
        // Remove default AvaloniaEdit URL behavior (blue text/underline)
        UrlEditor.TextArea.TextView.ElementGenerators.Clear();
        RequestHeadersEditor.TextArea.TextView.ElementGenerators.Clear();
        ResponseHeadersEditor.TextArea.TextView.ElementGenerators.Clear();
        RequestBodyEditor.TextArea.TextView.ElementGenerators.Clear();
        ResponseBodyEditor.TextArea.TextView.ElementGenerators.Clear();
        
        // Replace standard KeyDown with a Tunneling (preview) event handler
        UrlEditor.TextArea.AddHandler(InputElement.KeyDownEvent, UrlEditor_KeyDown_Tunnel, RoutingStrategies.Tunnel);
        
        // Remove the default "New Line" behavior from AvaloniaEdit
        foreach (var binding in UrlEditor.TextArea.DefaultInputHandler.KeyBindings)
        {
            if (binding.Gesture?.Key == Key.Enter || binding.Gesture?.Key == Key.Return)
            {
                UrlEditor.TextArea.DefaultInputHandler.KeyBindings.Remove(binding);
                break;
            }
        }
        
        UrlEditor.TextArea.TextView.LineTransformers.Add(colorizer);
        RequestHeadersEditor.TextArea.TextView.LineTransformers.Add(colorizer);
        RequestBodyEditor.TextArea.TextView.LineTransformers.Add(colorizer);
        
        RequestHeadersEditor.TextArea.TextView.LineTransformers.Add(_headerKeyColorizer);
        ResponseHeadersEditor.TextArea.TextView.LineTransformers.Add(_headerKeyColorizer);
        
        RequestHeadersEditor.TextArea.TextView.LineTransformers.Add(_headersErrorColorizer);
        RequestBodyEditor.TextArea.TextView.LineTransformers.Add(_bodyErrorColorizer);
        
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

    private void IndentComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (RequestBodyEditor != null)
        {
            FormatEditorContent(RequestBodyEditor);
        }
        if (ResponseBodyEditor != null)
        {
            FormatEditorContent(ResponseBodyEditor);
        }
    }

    private async void ManageEnvButton_Click(object? sender, RoutedEventArgs e)
    {
        var envWindow = new EnvironmentWindow(_envService);
        await envWindow.ShowDialog(this);
    }

    private void HistoryListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (HistoryListBox.SelectedItem is HistoryItem item)
        {
            // Set Request UI
            MethodComboBox.SelectedItem = MethodComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content?.ToString() == item.Method) ?? MethodComboBox.Items.Cast<ComboBoxItem>().First();
            UrlEditor.Text = item.Url;
            RequestHeadersEditor.Text = item.RequestHeaders;
            RequestBodyEditor.Text = item.RequestBody;

            // Set Response UI
            StatusTextBlock.Text = $"{item.StatusCode} {item.ReasonPhrase}";
            StatusTextBlock.Foreground = item.IsSuccess ? Avalonia.Media.Brushes.LightGreen : Avalonia.Media.Brushes.LightCoral;
            TimeTextBlock.Text = $"{item.ElapsedMilliseconds} ms";
            SizeTextBlock.Text = $"{item.SizeBytes} B";
            ResponseHeadersEditor.Text = item.ResponseHeaders;
            ResponseBodyEditor.Text = item.ResponseBody;
        }
    }

    private void ClearHistoryButton_Click(object? sender, RoutedEventArgs e)
    {
        _historyService.ClearHistory();
    }

    private void UrlEditor_KeyDown_Tunnel(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter || e.Key == Key.Return)
        {
            e.Handled = true; // Prevent new line
            SendButton_Click(sender, new RoutedEventArgs()); // Optionally trigger a send
        }
    }

    private async void SendButton_Click(object? sender, RoutedEventArgs e)
    {
        SendButton.IsEnabled = false;
        StatusTextBlock.Text = "Sending...";
        TimeTextBlock.Text = "--- ms";
        ResponseBodyEditor.Text = string.Empty;
        ResponseHeadersEditor.Text = string.Empty;

        var url = _envService.ReplaceVariables(UrlEditor.Text ?? string.Empty);
        var methodStr = (MethodComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? AppConstants.DefaultMethod;
        var method = new HttpMethod(methodStr);
        var headersText = _envService.ReplaceVariables(RequestHeadersEditor.Text ?? string.Empty);
        var bodyText = _envService.ReplaceVariables(RequestBodyEditor.Text ?? string.Empty);

        try
        {
            var response = await _httpService.SendRequestAsync(method, url, headersText, bodyText);

            var historyItem = new HistoryItem
            {
                Method = methodStr,
                Url = url,
                RequestHeaders = headersText,
                RequestBody = bodyText,
                StatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                IsSuccess = response.IsSuccess,
                ElapsedMilliseconds = response.ElapsedMilliseconds,
                SizeBytes = response.SizeBytes,
                ResponseHeaders = response.Headers,
                ResponseBody = response.Body
            };

            // Update UI
            Dispatcher.UIThread.Post(() =>
            {
                _historyService.AddEntry(historyItem);
                
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
        var errorColorizer = editor == RequestBodyEditor ? _bodyErrorColorizer : _headersErrorColorizer;
        
        errorColorizer.ClearError();
        editor.TextArea.TextView.Redraw();
        
        if (string.IsNullOrWhiteSpace(text))
        {
            DataValidationErrors.SetErrors(editor, null);
            return;
        }

        try
        {
            if (editor == RequestBodyEditor || editor == ResponseBodyEditor)
            {
                var indentString = (IndentComboBox?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "  ";
                editor.Text = TextFormatter.FormatBody(text, indentString);
            }
            else if (editor == RequestHeadersEditor || editor == ResponseHeadersEditor)
            {
                editor.Text = TextFormatter.FormatHeaders(text);
            }
            
            // Clear any previous errors on success
            DataValidationErrors.SetErrors(editor, null);
        }
        catch (JsonException ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
            if (ex.LineNumber.HasValue && ex.BytePositionInLine.HasValue)
            {
                int offset = GetErrorOffset(editor, ex.LineNumber.Value, ex.BytePositionInLine.Value);
                errorColorizer.SetError(Math.Max(0, offset - 1), 3);
                editor.TextArea.TextView.Redraw();
            }
        }
        catch (System.Xml.XmlException ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
            int offset = GetErrorOffset(editor, ex.LineNumber - 1, ex.LinePosition - 1);
            errorColorizer.SetError(Math.Max(0, offset - 1), 3);
            editor.TextArea.TextView.Redraw();
        }
        catch (HeaderFormatException ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
            int offset = GetErrorOffset(editor, ex.LineNumber, ex.CharPosition);
            errorColorizer.SetError(Math.Max(0, offset - 1), 3);
            editor.TextArea.TextView.Redraw();
        }
        catch (Exception ex)
        {
            DataValidationErrors.SetErrors(editor, new[] { ex.Message });
        }
    }

    private int GetErrorOffset(TextEditor editor, long lineIndex, long charPosition)
    {
        int line = (int)lineIndex + 1;
        int column = (int)charPosition + 1;
        
        if (line < 1) line = 1;
        if (line > editor.Document.LineCount) line = editor.Document.LineCount;
        
        var documentLine = editor.Document.GetLineByNumber(line);
        if (column < 1) column = 1;
        if (column > documentLine.Length + 1) column = documentLine.Length + 1;
        
        return documentLine.Offset + column - 1;
    }
}
