namespace ArenaInteractive.Tests;

using NUnit.Framework;
using System.Collections;
using System;
using Definitions;
using System.Linq;

internal static class ValidationTestData
{
    public static IEnumerable TestCases
    {
        get
        {
            var validInput = new Input
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

            var validOptions = new Options
            {
                PersonalAccessToken = "Test1234",
            };

            yield return new TestCaseData(validInput, validOptions).Returns(true);

            // Invalid input cases
            yield return new TestCaseData(validInput with { Sender = null }, validOptions).Returns(false);
            yield return new TestCaseData(validInput with { CustomerId = Guid.Empty }, validOptions).Returns(false);
            yield return new TestCaseData(validInput with { ServiceId = Guid.Empty }, validOptions).Returns(false);
            yield return new TestCaseData(validInput with { Content = string.Empty }, validOptions).Returns(false);
            yield return new TestCaseData(validInput with { Content = string.Join(string.Empty, Enumerable.Range(0, 1001).Select(_ => "A")) }, validOptions).Returns(false);
            yield return new TestCaseData(validInput with { Recipients = Array.Empty<Recipient>() }, validOptions).Returns(false);
            yield return new TestCaseData(validInput with { Recipients = new Recipient[] { new Recipient { Address = string.Empty, Personalization = Array.Empty<Personalization>() } } }, validOptions).Returns(false);

            // Invalid options cases
            yield return new TestCaseData(validInput, validOptions with { PersonalAccessToken = string.Empty }).Returns(false);
            yield return new TestCaseData(validInput, validOptions with { AllowedSendTimeStart = "12:00:00", AllowedSendTimeEnd = "11:59:59" }).Returns(false);
            yield return new TestCaseData(validInput, validOptions with { CustomerData = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "A")) }).Returns(false);
            yield return new TestCaseData(validInput, validOptions with { DlrUrl = "not-a-valid-uri" }).Returns(false);
            yield return new TestCaseData(validInput, validOptions with { RequestId = string.Join(string.Empty, Enumerable.Range(0, 1001).Select(_ => "A")) }).Returns(false);
            yield return new TestCaseData(validInput, validOptions with { SendDateTime = DateTime.UtcNow.AddMinutes(1) }).Returns(false);
        }
    }
}