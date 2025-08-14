using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArenaInteractive.SmartDialog.SendSmartMessage.Handlers;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace ArenaInteractive.SmartDialog.SendSmartMessage.Tests.Handlers;

internal class RetryHttpMessageHandlerTests
{
    private readonly Mock<HttpClientHandler> mockedInnerHandler;
    private readonly HttpClient httpClient;

    private const string HttpHandlerMockedMethodName = "SendAsync";

    public RetryHttpMessageHandlerTests()
    {
        mockedInnerHandler = new Mock<HttpClientHandler>();
        httpClient = new HttpClient(new RetryHttpMessageHandler(mockedInnerHandler.Object))
        {
            BaseAddress = new Uri("https://www.test.com/fake/"),
        };
    }

    [SetUp]
    public void ClearMockCalls()
    {
        mockedInnerHandler.Invocations.Clear();
    }

    [TestCaseSource(typeof(RetryHttpMessageHandlerTestData), nameof(RetryHttpMessageHandlerTestData.TestCases))]
    public async Task SendAsync_HttpStatusCodeReturned_ExpectedAmountOfCallsToInnerHandler(HttpStatusCode responseStatusCode, int expectedAmountOfCalls = 1)
    {
        mockedInnerHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                HttpHandlerMockedMethodName,
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(responseStatusCode));

        await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, "test"));

        mockedInnerHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(HttpHandlerMockedMethodName, Times.Exactly(expectedAmountOfCalls), ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put), ItExpr.IsAny<CancellationToken>());
    }
}