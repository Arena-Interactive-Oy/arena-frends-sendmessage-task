namespace ArenaInteractive.Tests;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DTOs;
using Definitions;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using Moq;

[TestFixture]
internal class UnitTests
{
    private readonly Input validInput;
    private readonly Options validOptions;

    private readonly Mock<HttpClient> mockhttpClient;

    public UnitTests()
    {
        validInput = new Input
        {
            Sender = "TestSender",
            Content = "Hello $(test)",
            CustomerId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Recipients = new Recipient[]
            {
                new Recipient
                {
                    Address = "358101001234",
                    Personalization = new Personalization[]
                    {
                        new Personalization { Name = "test", Value = "world" },
                    },
                },
            },
        };

        validOptions = new Options
        {
            PersonalAccessToken = "Test1234",
            ThrowErrorOnFailure = false,
        };

        mockhttpClient = new Mock<HttpClient>(MockBehavior.Strict);
        SmartDialog.smartDialogHttpClient = mockhttpClient.Object;
    }

    public UnitTests(Input validInput)
    {
        this.validInput = validInput;
    }

    [Test]
    public async Task SendSmartMessage_ValidParameters_SuccessfulResponseReturned()
    {
        var responseObject = new SendResponse
        {
            Id = Guid.NewGuid().ToString(),
            MessagePartCount = 1,
            RecipientCount = 1,
            SendDateTimeEstimate = DateTime.UtcNow,
            Warnings = Array.Empty<string>(),
        };

        mockhttpClient
            .Setup(client => client.SendAsync(It.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(responseObject) });

        var result = await SmartDialog.SendSmartMessage(validInput, validOptions, CancellationToken.None);
        Assert.True(result.Success);
    }

    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.TooManyRequests)]
    public async Task SendSmartMessage_HandledErrorCases_BadRequestResponseReturned(HttpStatusCode statusCodeToReturn)
    {
        var responseObject = new SendResponse
        {
            ErrorMessage = "Something went wrong with validation",
        };

        mockhttpClient
            .Setup(client => client.SendAsync(It.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(statusCodeToReturn) { Content = JsonContent.Create(responseObject) });

        var result = await SmartDialog.SendSmartMessage(validInput, validOptions, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.Error?.Message);
    }

    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task SendSmartMessage_UnknownErrorCases_UnhandledStatusCodeReturned(HttpStatusCode statusCodeToReturn)
    {
        mockhttpClient
            .Setup(client => client.SendAsync(It.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(statusCodeToReturn));

        var result = await SmartDialog.SendSmartMessage(validInput, validOptions, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.Error?.Message);
    }

    [TestCaseSource(typeof(ValidationTestData), nameof(ValidationTestData.TestCases))]
    public async Task<bool> SendSmartMessage_TaskParametersValidationFailed_FailedResultReturned(Input input, Options options)
    {
        var validSendResponse = new SendResponse
        {
            Id = Guid.NewGuid().ToString(),
            RecipientCount = 1,
            MessagePartCount = 1,
            SendDateTimeEstimate = DateTime.UtcNow,
            Warnings = Array.Empty<string>(),
        };

        mockhttpClient
            .Setup(client => client.SendAsync(It.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(validSendResponse) });

        var result = await SmartDialog.SendSmartMessage(input, options, CancellationToken.None);
        return result.Success;
    }
}
