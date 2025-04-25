namespace ArenaInteractive.Handlers;

using System.Linq;
using System.Net;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal class RetryHttpMessageHandler : DelegatingHandler
{
    private const int MaxRetries = 10;

    public RetryHttpMessageHandler()
        : base(new HttpClientHandler())
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestCount = 0;
        do
        {
            requestCount++;
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode && !Constants.HandledStatusCodes.Contains(response.StatusCode) && requestCount < MaxRetries)
                {
                    continue;
                }

                return response;
            }
            catch (Exception)
            {
                // noop
            }
        }
        while (requestCount < MaxRetries);

        return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
    }
}