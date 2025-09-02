namespace ArenaInteractive.SmartDialog.SendSmartMessage.Tests;

using System;
using System.Collections;
using System.Linq;
using Definitions;
using NUnit.Framework;

internal static class ValidationTestData
{
    public static IEnumerable TestCases
    {
        get
        {
            var i = 0;

            yield return new TestCaseData(CreateInput(), CreateOptions()).Returns(true).SetName($"{nameof(ValidationTestData)}_{i++}");

            // Invalid input cases
            yield return new TestCaseData(CreateInput(null), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(customerId: Guid.Empty), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(serviceId: Guid.Empty), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(content: string.Empty), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(content: string.Join(string.Empty, Enumerable.Range(0, 1001).Select(_ => "A"))), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(recipients: Array.Empty<Recipient>()), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(recipients: new Recipient[] { new Recipient { Address = string.Empty, Personalization = Array.Empty<Personalization>() } }), CreateOptions()).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");

            // Invalid options cases
            yield return new TestCaseData(CreateInput(), CreateOptions(string.Empty)).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(), CreateOptions(allowedSendTimeStart: "12:00:00", allowedSendTimeEnd: "11:59:59")).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(), CreateOptions(customerData: string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "A")))).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(), CreateOptions(dlrUrl: "not-a-valid-uri")).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(), CreateOptions(requestId: string.Join(string.Empty, Enumerable.Range(0, 1001).Select(_ => "A")))).Returns(false).SetName($"{nameof(ValidationTestData)}_{i++}");
            yield return new TestCaseData(CreateInput(), CreateOptions(sendDateTime: DateTime.UtcNow.AddMinutes(1)) ).Returns(false).SetName($"{nameof(ValidationTestData)}_{i}");
        }
    }

    private static Input CreateInput(string sender = "TestSender", string content = "Hello world",
        Guid? customerId = null, Guid? serviceId = null, Recipient[] recipients = null)
    {
        return new Input
        {
            Sender = sender,
            Content = content,
            CustomerId = customerId ?? Guid.NewGuid(),
            ServiceId = serviceId ?? Guid.NewGuid(),
            Recipients = recipients ?? new Recipient[]
            {
                new Recipient
                {
                    Address = "358101001234",
                },
            },
        };
    }

    private static Options CreateOptions(string pat = "Test1234", bool throwErrorOnFailure = false, string allowedSendTimeStart = null, string allowedSendTimeEnd = null,
        string customerData = null, string dlrUrl = null, string requestId = null, DateTime? sendDateTime = null)
    {
        return new Options
        {
            PersonalAccessToken = pat,
            ThrowErrorOnFailure = throwErrorOnFailure,
            AllowedSendTimeStart = allowedSendTimeStart,
            AllowedSendTimeEnd = allowedSendTimeEnd,
            CustomerData = customerData,
            DlrUrl = dlrUrl,
            RequestId = requestId,
            SendDateTime = sendDateTime
        };
    }
}