using System.Threading.Tasks;

namespace TGenApiClient.Core.Contracts;

/// <summary>
/// Defines a service capable of asynchronous HTTP request execution.
/// </summary>
public interface IRequestExecutionService
{
    /// <summary>
    /// Asynchronously executes a defined HTTP request and records the result.
    /// </summary>
    /// <param name="method">The HTTP Method (e.g. GET, POST).</param>
    /// <param name="url">The fully-qualified API URL.</param>
    /// <param name="headersText">Raw multiline text of the headers.</param>
    /// <param name="bodyText">Raw text of the request body.</param>
    /// <returns>A unified <see cref="Models.HttpResponseData"/> instance encapsulating the status, headers, and body.</returns>
    Task<Models.HttpResponseData> SendRequestAsync(System.Net.Http.HttpMethod method, string url, string headersText, string bodyText);
}
