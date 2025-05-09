namespace ArenaInteractive.Definitions;
using System;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// True if message was queued successfully
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// Smart send ID
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Total recipient count
    /// </summary>
    public int? RecipientCount { get; private set; }

    /// <summary>
    /// Total message part count
    /// </summary>
    public int? MessagePartCount { get; private set; }

    /// <summary>
    /// Rough estimate for when message has been sent to all recipients
    /// </summary>
    public DateTime? SendDateTimeEstimate { get; private set; }

    /// <summary>
    /// Warnings related to recipient-specific customizations
    /// </summary>
    public string[] Warnings { get; private set; }

    /// <summary>
    /// Error, in case sending was unsuccessful.
    /// </summary>
    public Error Error { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="id">Smart send id</param>
    /// <param name="recipientCount">Recipient count</param>
    /// <param name="messagePartCount">Message part count</param>
    /// <param name="sendDateTimeEstimate">Send date time estimate</param>
    /// <param name="warnings">Warnings</param>
    public Result(string id, int recipientCount, int messagePartCount, DateTime sendDateTimeEstimate, string[] warnings)
    {
        Id = id;
        RecipientCount = recipientCount;
        MessagePartCount = messagePartCount;
        SendDateTimeEstimate = sendDateTimeEstimate;
        Warnings = warnings;
        Success = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="error">Error details</param>
    public Result(Error error)
    {
        Error = error;
        Success = false;
    }
}

/// <summary>
/// Class containing error details
/// </summary>
public class Error
{
    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Additional info
    /// </summary>
    public ErrorAdditionalInfo AdditionalInfo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="message">Detailed error message</param>
    /// <param name="additionalInfo">Additional information about the error</param>
    public Error(string message, ErrorAdditionalInfo additionalInfo)
    {
        Message = message;
        AdditionalInfo = additionalInfo;
    }
}

/// <summary>
/// Class containing additional information about an error
/// </summary>
public class ErrorAdditionalInfo
{
    /// <summary>
    /// HTTP status code in the API response
    /// </summary>
    public int HttpStatusCode { get; set; }

    /// <summary>
    /// HTTP response body, if any, from the API response
    /// </summary>
    public string ResponseBody { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorAdditionalInfo"/> class.
    /// </summary>
    /// <param name="httpStatusCode">HTTP status code from API</param>
    /// <param name="responseBody">HTTP response body from API</param>
    public ErrorAdditionalInfo(int httpStatusCode, string responseBody)
    {
        HttpStatusCode = httpStatusCode;
        ResponseBody = responseBody;
    }
}
