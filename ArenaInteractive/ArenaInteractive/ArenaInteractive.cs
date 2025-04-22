namespace ArenaInteractive;

using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DTOs;
using Extensions;
using System;
using System.Net.Http;
using Handlers;
using System.ComponentModel;
using System.Threading;
using Definitions;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class SmartDialog
{
    internal static HttpClient SmartDialogHttpClient = CreateSmartDialogHttpClient();

    /// <summary>
    /// Send message using Smart parameters
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/ArenaInteractive).
    /// </summary>
    /// <param name="input">Message send parameters</param>
    /// <param name="options">Optional parameters and controls</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { string Id, int RecipientCount, int MessagePartCount, DateTime SendDateTimeEstimate, string[] Warnings }.</returns>
    public static async Task<Result> SendSmartMessage([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        var inputValidationMessage = input.Validate();
        if (inputValidationMessage != null)
        {
            return new Result(inputValidationMessage);
        }

        var optionsValidationMessage = options.Validate();
        if (optionsValidationMessage != null)
        {
            return new Result(optionsValidationMessage);
        }

        var smartSendMessage = new SmartSendMessage(
            input.Sender,
            input.Content,
            input.Recipients.Select(r => new SmartSendMessageRecipient(r.Address, r.Personalization)).ToArray(),
            options.DlrUrl,
            options.CustomerData,
            options.SendDateTime,
            options.AllowedSendTimeStart,
            options.AllowedSendTimeEnd,
            options.AllowedSendDays,
            options.RequestId,
            options.UnicodeCharacterHandlingPolicy);

        var response = await SmartDialogHttpClient.PutAsJsonAsync($"messages?personalAccessToken={options.PersonalAccessToken}&customerId={input.CustomerId}", smartSendMessage, SmartDialogSourceGenerationContext.Default.SmartSendMessage, cancellationToken);
        if (!response.IsSuccessStatusCode && !Constants.HandledStatusCodes.Contains(response.StatusCode))
        {
            var responseBodyAsString = await response.Content.ReadAsStringAsync(cancellationToken);
            return new Result($"Sending failed, unknown statusCode: {(int)response.StatusCode}, response body: {responseBodyAsString}");
        }

        var responseObject = await response.Content.ReadFromJsonAsync<SendResponse>(SmartDialogSourceGenerationContext.Default.SendResponse, cancellationToken);
        if (responseObject == null)
        {
            return new Result("Sending failed, response body is empty");
        }

        return responseObject.Successful
            ? new Result(
                responseObject.Id,
                responseObject.RecipientCount!.Value,
                responseObject.MessagePartCount!.Value,
                responseObject.SendDateTimeEstimate!.Value,
                responseObject.Warnings)
            : new Result(responseObject.ErrorMessage ?? "Unknown error");
    }

    internal static HttpClient CreateSmartDialogHttpClient(HttpMessageHandler primaryMessageHandler = null)
    {
        var httpClient = primaryMessageHandler != null
            ? new HttpClient(primaryMessageHandler)
            : new HttpClient(new RetryHttpMessageHandler());

        httpClient.BaseAddress = new Uri("https://api.arena.fi/messaging-gateway/v1/", UriKind.Absolute);

        return httpClient;
    }
}
