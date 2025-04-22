namespace ArenaInteractive.Tests;

using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DTOs;

internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCodeToReturn;
    private readonly SendResponse _sendResponseObjectToReturn;

    public FakeHttpMessageHandler(HttpStatusCode statusCodeToReturn, SendResponse sendResponseObjectToReturn)
    {
        _statusCodeToReturn = statusCodeToReturn;
        _sendResponseObjectToReturn = sendResponseObjectToReturn;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseObject = _sendResponseObjectToReturn != null
            ? new HttpResponseMessage(_statusCodeToReturn)
            {
                Content = new StringContent(JsonSerializer.Serialize(
                    _sendResponseObjectToReturn,
                    SmartDialogSourceGenerationContext.Default.SendResponse)),
            }
            : new HttpResponseMessage(_statusCodeToReturn);
        return Task.FromResult(responseObject);
    }
}