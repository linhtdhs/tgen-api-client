using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tgenapiclient.Models;

namespace tgenapiclient.Services;


public class HttpService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<HttpResponseData> SendRequestAsync(HttpMethod method, string url, string headersText, string bodyText)
    {
        var request = new HttpRequestMessage(method, url);

        // Add headers
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

        var size = System.Text.Encoding.UTF8.GetByteCount(responseBody ?? string.Empty);

        return new HttpResponseData
        {
            StatusCode = (int)response.StatusCode,
            ReasonPhrase = response.ReasonPhrase ?? string.Empty,
            ElapsedMilliseconds = sw.ElapsedMilliseconds,
            SizeBytes = size,
            Body = responseBody ?? string.Empty,
            Headers = sbHeaders.ToString(),
            IsSuccess = (int)response.StatusCode >= 200 && (int)response.StatusCode < 300
        };
    }
}
