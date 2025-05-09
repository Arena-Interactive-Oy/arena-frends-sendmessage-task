namespace ArenaInteractive.Tests;

using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DTOs;

internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode statusCodeToReturn;
    private readonly SendResponse sendResponseObjectToReturn;

    public FakeHttpMessageHandler(HttpStatusCode statusCodeToReturn, SendResponse sendResponseObjectToReturn)
    {
        this.statusCodeToReturn = statusCodeToReturn;
        this.sendResponseObjectToReturn = sendResponseObjectToReturn;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseObject = sendResponseObjectToReturn != null
            ? new HttpResponseMessage(statusCodeToReturn)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    sendResponseObjectToReturn,
                    SmartDialogSourceGenerationContext.Default.SendResponse)),
            }
            : new HttpResponseMessage(statusCodeToReturn);
        return Task.FromResult(responseObject);
    }
}