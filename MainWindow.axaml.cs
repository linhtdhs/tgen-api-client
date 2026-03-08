using System;
using System.Net.Http;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using tgenapiclient.Models;
using tgenapiclient.Services;
using tgenapiclient.Utils;

namespace tgenapiclient;

public partial class MainWindow : Window
{
    private readonly HttpService _httpService = new HttpService();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SendButton_Click(object? sender, RoutedEventArgs e)
    {
        SendButton.IsEnabled = false;
        StatusTextBlock.Text = "Sending...";
        TimeTextBlock.Text = "--- ms";
        ResponseBodyTextBox.Text = string.Empty;
        ResponseHeadersTextBox.Text = string.Empty;

        var url = UrlTextBox.Text;
        var methodStr = (MethodComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "GET";
        var method = new HttpMethod(methodStr);
        var headersText = RequestHeadersTextBox.Text;
        var bodyText = RequestBodyTextBox.Text;

        try
        {
            var response = await _httpService.SendRequestAsync(method, url ?? string.Empty, headersText ?? string.Empty, bodyText ?? string.Empty);

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

                ResponseBodyTextBox.Text = response.Body;
                ResponseHeadersTextBox.Text = response.Headers;
            });
        }
        catch (Exception ex)
        {
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBlock.Text = "Error";
                StatusTextBlock.Foreground = Avalonia.Media.Brushes.Red;
                ResponseBodyTextBox.Text = ex.ToString();
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

    private void TextBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            FormatTextBoxContent(textBox);
        }
    }

    private void TextBox_PastingFromClipboard(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Delay formatting slightly to allow the paste to complete
            Dispatcher.UIThread.Post(() => FormatTextBoxContent(textBox));
        }
    }

    private void FormatTextBoxContent(TextBox textBox)
    {
        var text = textBox.Text;
        if (string.IsNullOrWhiteSpace(text))
        {
            DataValidationErrors.SetErrors(textBox, null);
            return;
        }

        try
        {
            if (textBox == RequestBodyTextBox)
            {
                textBox.Text = TextFormatter.FormatBody(text);
            }
            else if (textBox == RequestHeadersTextBox)
            {
                textBox.Text = TextFormatter.FormatHeaders(text);
            }
            
            // Clear any previous errors on success
            DataValidationErrors.SetErrors(textBox, null);
        }
        catch (JsonException ex)
        {
            DataValidationErrors.SetErrors(textBox, new[] { ex.Message });
            ErrorHighlighter.Highlight(textBox, text, ex.LineNumber, ex.BytePositionInLine);
        }
        catch (System.Xml.XmlException ex)
        {
            DataValidationErrors.SetErrors(textBox, new[] { ex.Message });
            ErrorHighlighter.Highlight(textBox, text, ex.LineNumber - 1, ex.LinePosition - 1);
        }
        catch (Exception ex)
        {
            DataValidationErrors.SetErrors(textBox, new[] { ex.Message });
        }
    }
}
