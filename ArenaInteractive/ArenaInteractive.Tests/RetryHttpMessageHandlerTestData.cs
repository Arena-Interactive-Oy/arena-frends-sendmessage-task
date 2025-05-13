namespace ArenaInteractive.Tests;

using System.Net;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;

internal static class RetryHttpMessageHandlerTestData
{
    public static IEnumerable TestCases
    {
        get
        {
            foreach (var httpStatusCode in Enum.GetValues<HttpStatusCode>())
            {
                var handled = Constants.HandledStatusCodes.Contains(httpStatusCode);
                var successStatusCode = (int)httpStatusCode >= 200 && (int)httpStatusCode <= 299;

                yield return new TestCaseData(httpStatusCode, handled || successStatusCode ? 1 : Constants.MaxRetries).SetName(httpStatusCode.ToString());
            }
        }
    }
}