namespace ArenaInteractive.Tests;

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DTOs;
using Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    private readonly Input _validInput;
    private readonly Options _validOptions;

    public UnitTests()
    {
        _validInput = new Input
        {
            Sender = "TestSender",
            Content = "Hello world",
            CustomerId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Recipients = new Recipient[]
            {
                new Recipient
                {
                    Address = "358101001234",
                },
            },
        };

        _validOptions = new Options
        {
            PersonalAccessToken = "Test1234",
        };
    }

    [Test]
    public async Task SendSmartMessage_ValidParameters_SuccessfulResponseReturned()
    {
        SmartDialog.SmartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, new SendResponse
        {
            Id = Guid.NewGuid().ToString(),
            MessagePartCount = 1,
            RecipientCount = 1,
            SendDateTimeEstimate = DateTime.UtcNow,
            Warnings = Array.Empty<string>(),
        }));

        var result = await SmartDialog.SendSmartMessage(_validInput, _validOptions, CancellationToken.None);
        Assert.True(result.Success);
    }

    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.TooManyRequests)]
    public async Task SendSmartMessage_HandledErrorCases_BadRequestResponseReturned(HttpStatusCode statusCodeToReturn)
    {
        SmartDialog.SmartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(statusCodeToReturn, new SendResponse
        {
            ErrorMessage = "Something went wrong with validation",
        }));

        var result = await SmartDialog.SendSmartMessage(_validInput, _validOptions, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.ErrorMessage);
    }

    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task SendSmartMessage_UnknownErrorCases_UnhandledStatusCodeReturned(HttpStatusCode statusCodeToReturn)
    {
        SmartDialog.SmartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(statusCodeToReturn, null));

        var result = await SmartDialog.SendSmartMessage(_validInput, _validOptions, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.ErrorMessage);
    }

    [TestCaseSource(typeof(ValidationTestData), nameof(ValidationTestData.TestCases))]
    public async Task<bool> SendSmartMessage_TaskParametersValidationFailed_FailedResultReturned(Input input, Options options)
    {
        var validSendResponse = new SendResponse
        {
            Id = Guid.NewGuid().ToString(), RecipientCount = 1, MessagePartCount = 1,
            SendDateTimeEstimate = DateTime.UtcNow,
            Warnings = Array.Empty<string>(),
        };
        SmartDialog.SmartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, validSendResponse));

        var result = await SmartDialog.SendSmartMessage(input, options, CancellationToken.None);
        return result.Success;
    }
}
