namespace ArenaInteractive;

using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Globalization;
using System.Linq;
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
    internal static HttpClient smartDialogHttpClient = CreateSmartDialogHttpClient();

    /// <summary>
    /// Send message using Smart parameters
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/ArenaInteractive).
    /// </summary>
    /// <param name="input">Message send parameters</param>
    /// <param name="options">Optional parameters and controls</param>
    /// <param name="cancellationToken">Cancellation token given by Frends.</param>
    /// <returns>Object { bool Success, string Id, int RecipientCount, int MessagePartCount, DateTime SendDateTimeEstimate, string[] Warnings }.</returns>
    public static async Task<Result> SendSmartMessage([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        var inputValidationMessage = input.Validate();
        if (inputValidationMessage != null)
        {
            if (options.ThrowErrorOnFailure)
            {
                throw new Exception(BuildErrorMessage(options.ErrorMessageOnFailure, inputValidationMessage));
            }

            return new Result(new Error(BuildErrorMessage(options.ErrorMessageOnFailure, inputValidationMessage), null));
        }

        var optionsValidationMessage = options.Validate();
        if (optionsValidationMessage != null)
        {
            if (options.ThrowErrorOnFailure)
            {
                throw new Exception(BuildErrorMessage(options.ErrorMessageOnFailure, optionsValidationMessage));
            }

            return new Result(new Error(BuildErrorMessage(options.ErrorMessageOnFailure, optionsValidationMessage), null));
        }

        var smartSendMessage = new SmartSendMessage(
            input.Sender,
            input.Content,
            input.Recipients.Select(r => new SmartSendMessageRecipient(r.Address, r.Personalization?.ToDictionary(p => p.Name, p => p.Value))).ToArray(),
            options.DlrUrl,
            options.CustomerData,
            options.SendDateTime,
            TimeSpan.TryParse(options.AllowedSendTimeStart, CultureInfo.InvariantCulture, out var start) ? start : null,
            TimeSpan.TryParse(options.AllowedSendTimeEnd, CultureInfo.InvariantCulture, out var end) ? end : null,
            options.AllowedSendDays,
            !string.IsNullOrWhiteSpace(options.RequestId) ? options.RequestId : Guid.NewGuid().ToString(),
            options.UnicodeCharacterHandlingPolicy);

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "messages");
        httpRequestMessage.Content = JsonContent.Create(smartSendMessage, new MediaTypeHeaderValue(MediaTypeNames.Application.Json), SmartDialogSourceGenerationContext.Default.Options);
        httpRequestMessage.Headers.TryAddWithoutValidation("Personal-Access-Token", options.PersonalAccessToken);
        httpRequestMessage.Headers.TryAddWithoutValidation("Customer-Id", input.CustomerId.ToString());
        httpRequestMessage.Headers.TryAddWithoutValidation("Service-Id", input.ServiceId.ToString());

        var response = await smartDialogHttpClient.SendAsync(httpRequestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode && !Constants.HandledStatusCodes.Contains(response.StatusCode))
        {
            var responseBodyAsString = await response.Content.ReadAsStringAsync(cancellationToken);
            var message = $"Sending failed, unknown statusCode: {(int)response.StatusCode}, response body: {responseBodyAsString}";
            var errorMessage = BuildErrorMessage(options.ErrorMessageOnFailure, message);

            if (options.ThrowErrorOnFailure)
            {
                throw new Exception(errorMessage);
            }

            return new Result(new Error(BuildErrorMessage(options.ErrorMessageOnFailure, errorMessage), new ErrorAdditionalInfo((int)response.StatusCode, responseBodyAsString)));
        }

        var responseObject = await response.Content.ReadFromJsonAsync(SmartDialogSourceGenerationContext.Default.SendResponse, cancellationToken);
        if (responseObject == null)
        {
            var errorMessage = BuildErrorMessage(options.ErrorMessageOnFailure, "Sending failed, response body is empty");
            return new Result(new Error(BuildErrorMessage(options.ErrorMessageOnFailure, errorMessage), new ErrorAdditionalInfo((int)response.StatusCode, null)));
        }

        return responseObject.Successful
            ? new Result(
                responseObject.Id,
                responseObject.RecipientCount!.Value,
                responseObject.MessagePartCount!.Value,
                responseObject.SendDateTimeEstimate!.Value,
                responseObject.Warnings)
            : new Result(new Error(BuildErrorMessage(options.ErrorMessageOnFailure, responseObject.ErrorMessage ?? "Unknown error"), new ErrorAdditionalInfo((int)response.StatusCode, JsonSerializer.Serialize(responseObject))));
    }

    internal static HttpClient CreateSmartDialogHttpClient(HttpMessageHandler primaryMessageHandler = null)
    {
        var httpClient = primaryMessageHandler != null
            ? new HttpClient(primaryMessageHandler)
            : new HttpClient(new RetryHttpMessageHandler());

        httpClient.BaseAddress = new Uri("https://api.arena.fi/messaging-gateway/v1/", UriKind.Absolute);

        return httpClient;
    }

    private static string BuildErrorMessage(string errorMessageOnFailure, string taskErrorMessage)
    {
        return !string.IsNullOrWhiteSpace(errorMessageOnFailure)
            ? $"{errorMessageOnFailure}: {taskErrorMessage}"
            : taskErrorMessage;
    }
}
