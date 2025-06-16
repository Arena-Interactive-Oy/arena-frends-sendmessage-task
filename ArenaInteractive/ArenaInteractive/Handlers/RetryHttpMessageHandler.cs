namespace ArenaInteractive.Handlers;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal class RetryHttpMessageHandler : DelegatingHandler
{
    public RetryHttpMessageHandler(HttpClientHandler innerHandler)
        : base(innerHandler)
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestCount = 0;
        do
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode || Constants.HandledStatusCodes.Contains(response.StatusCode))
            {
                return response;
            }

            requestCount++;
        }
        while (requestCount < Constants.MaxRetries);

        return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
    }
}