using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace tgenapiclient;

public partial class MainWindow : Window
{
    private static readonly HttpClient _httpClient = new HttpClient();

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

        try
        {
            var request = new HttpRequestMessage(method, url);

            // Add headers
            var headersText = RequestHeadersTextBox.Text;
            if (!string.IsNullOrWhiteSpace(headersText))
            {
                var lines = headersText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var index = line.IndexOf(':');
                    if (index > 0)
                    {
                        var key = line.Substring(0, index).Trim();
                        var value = line.Substring(index + 1).Trim();
                        request.Headers.TryAddWithoutValidation(key, value);
                    }
                }
            }

            // Add body if applicable
            if (method != HttpMethod.Get && method != HttpMethod.Head)
            {
                var bodyText = RequestBodyTextBox.Text;
                if (!string.IsNullOrWhiteSpace(bodyText))
                {
                    request.Content = new StringContent(bodyText, Encoding.UTF8, "application/json"); // Basic default
                }
            }

            var sw = Stopwatch.StartNew();
            var response = await _httpClient.SendAsync(request);
            sw.Stop();

            var responseBody = await response.Content.ReadAsStringAsync();
            
            // Format headers
            var sbHeaders = new StringBuilder();
            foreach (var header in response.Headers)
            {
                sbHeaders.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            foreach (var header in response.Content.Headers)
            {
                sbHeaders.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            var size = System.Text.Encoding.UTF8.GetByteCount(responseBody);

            // Update UI
            Dispatcher.UIThread.Post(() =>
            {
                StatusTextBlock.Text = $"{(int)response.StatusCode} {response.ReasonPhrase}";
                if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
                    StatusTextBlock.Foreground = Avalonia.Media.Brushes.LightGreen;
                else
                    StatusTextBlock.Foreground = Avalonia.Media.Brushes.LightCoral;

                TimeTextBlock.Text = $"{sw.ElapsedMilliseconds} ms";
                SizeTextBlock.Text = $"{size} B";

                ResponseBodyTextBox.Text = responseBody;
                ResponseHeadersTextBox.Text = sbHeaders.ToString();
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
}
