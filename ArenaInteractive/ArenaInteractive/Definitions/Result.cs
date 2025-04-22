namespace ArenaInteractive.Definitions;
using System;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
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
    /// <param name="errorMessage">Error message from API</param>
    public Result(string errorMessage)
    {
        ErrorMessage = errorMessage;
        Success = false;
    }

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
    /// Error message, in case sending was unsuccessful.
    /// </summary>
    public string ErrorMessage { get; private set; }
}
