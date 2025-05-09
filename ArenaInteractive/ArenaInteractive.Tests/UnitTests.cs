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
    private readonly Input validInput;
    private readonly Options validOptions;

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
    }

    public UnitTests(Input validInput)
    {
        this.validInput = validInput;
    }

    [Test]
    public async Task SendSmartMessage_ValidParameters_SuccessfulResponseReturned()
    {
        SmartDialog.smartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, new SendResponse
        {
            Id = Guid.NewGuid().ToString(),
            MessagePartCount = 1,
            RecipientCount = 1,
            SendDateTimeEstimate = DateTime.UtcNow,
            Warnings = Array.Empty<string>(),
        }));

        var result = await SmartDialog.SendSmartMessage(validInput, validOptions, CancellationToken.None);
        Assert.True(result.Success);
    }

    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.TooManyRequests)]
    public async Task SendSmartMessage_HandledErrorCases_BadRequestResponseReturned(HttpStatusCode statusCodeToReturn)
    {
        SmartDialog.smartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(statusCodeToReturn, new SendResponse
        {
            ErrorMessage = "Something went wrong with validation",
        }));

        var result = await SmartDialog.SendSmartMessage(validInput, validOptions, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.Error?.Message);
    }

    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task SendSmartMessage_UnknownErrorCases_UnhandledStatusCodeReturned(HttpStatusCode statusCodeToReturn)
    {
        SmartDialog.smartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(statusCodeToReturn, null));

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
        SmartDialog.smartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, validSendResponse));

        var result = await SmartDialog.SendSmartMessage(input, options, CancellationToken.None);
        return result.Success;
    }
}
