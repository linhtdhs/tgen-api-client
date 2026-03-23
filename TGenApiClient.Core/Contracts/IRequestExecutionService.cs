using System.Threading.Tasks;

namespace TGenApiClient.Core.Contracts;

public interface IRequestExecutionService
{
    Task<Models.HttpResponseData> SendRequestAsync(System.Net.Http.HttpMethod method, string url, string headersText, string bodyText);
}
