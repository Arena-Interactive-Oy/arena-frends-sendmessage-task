namespace ArenaInteractive.Tests;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DTOs;
using Definitions;
using NUnit.Framework;

[TestFixture]
internal class UnitTests
{
    [Test]
    public async Task SendSmartMessage_ValidParameters_SuccessfulResponseReturned()
    {
        SmartDialog.SmartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, new SendResponse
        {
            Id = Guid.NewGuid().ToString(),
            MessagePartCount = 1,
            RecipientCount = 1,
            SendDateTimeEstimate = DateTime.UtcNow,
            Warnings = [],
        }));

        var input = new Input
        {
            Content = "Hello world",
            Recipients =
            [
                new Recipient
                {
                    Address = "358101001234",
                    Personalization = new Dictionary<string, string>(),
                },
            ],
            Sender = "Tester",
            ServiceId = Guid.NewGuid(),
        };

        var options = new Options { PersonalAccessToken = "test1234" };

        var result = await SmartDialog.SendSmartMessage(input, options, CancellationToken.None);
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

        var input = new Input
        {
            Content = "Hello world",
            Recipients =
            [
                new Recipient
                {
                    Address = "358101001234",
                    Personalization = new Dictionary<string, string>(),
                },
            ],
            Sender = "Tester",
            ServiceId = Guid.NewGuid(),
        };

        var options = new Options { PersonalAccessToken = "test1234" };

        var result = await SmartDialog.SendSmartMessage(input, options, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.ErrorMessage);
    }

    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.BadGateway)]
    public async Task SendSmartMessage_UnknownErrorCases_UnhandledStatusCodeReturned(HttpStatusCode statusCodeToReturn)
    {
        SmartDialog.SmartDialogHttpClient = SmartDialog.CreateSmartDialogHttpClient(new FakeHttpMessageHandler(statusCodeToReturn, null));

        var input = new Input
        {
            Content = "Hello world",
            Recipients =
            [
                new Recipient
                {
                    Address = "358101001234",
                    Personalization = new Dictionary<string, string>(),
                },
            ],
            Sender = "Tester",
            ServiceId = Guid.NewGuid(),
        };

        var options = new Options { PersonalAccessToken = "test1234" };

        var result = await SmartDialog.SendSmartMessage(input, options, CancellationToken.None);
        Assert.False(result.Success);
        Assert.IsNotEmpty(result.ErrorMessage);
    }
}
