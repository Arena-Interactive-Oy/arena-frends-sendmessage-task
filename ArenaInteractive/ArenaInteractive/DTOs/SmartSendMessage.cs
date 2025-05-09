namespace ArenaInteractive.DTOs;

using Definitions;
using System;

/// <summary>
/// Request object type for Smart Send Messages API
/// </summary>
public class SmartSendMessage
{
    /// <summary>
    /// Gets or sets Sender
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// Gets or sets Content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets Recipients array
    /// </summary>
    public SmartSendMessageRecipient[] Recipients { get; set; }

    /// <summary>
    /// Gets or sets DlrUrl
    /// </summary>
    public string DlrUrl { get; set; }

    /// <summary>
    /// Gets or sets CustomerData
    /// </summary>
    public string CustomerData { get; set; }

    /// <summary>
    /// Gets or sets SendDateTime
    /// </summary>
    public DateTime? SendDateTime { get; set; }

    /// <summary>
    /// Gets or sets AllowedSendTimeStart
    /// </summary>
    public TimeSpan? AllowedSendTimeStart { get; set; }

    /// <summary>
    /// Gets or sets AllowedSendTimeEnd
    /// </summary>
    public TimeSpan? AllowedSendTimeEnd { get; set; }

    /// <summary>
    /// Gets or sets AllowedSendDays array
    /// </summary>
    public DayOfWeek[] AllowedSendDays { get; set; }

    /// <summary>
    /// Gets or sets RequestId
    /// </summary>
    public string RequestId { get; set; }

    /// <summary>
    /// Gets or sets UnicodeCharacterHandlingPolicy
    /// </summary>
    public UnicodeCharacterHandlingPolicy UnicodeCharacterHandlingPolicy { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartSendMessage"/> class.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="content">Message content</param>
    /// <param name="recipients">Recipients array</param>
    /// <param name="dlrUrl">DlrUrl, if any</param>
    /// <param name="customerData">CustomerData, if any</param>
    /// <param name="sendDateTime">SendDateTime, if any</param>
    /// <param name="allowedSendTimeStart">Allowed send time range start, if any</param>
    /// <param name="allowedSendTimeEnd">Allowed send time range end, if any</param>
    /// <param name="allowedSendDays">Allowed send days, if restricted</param>
    /// <param name="requestId">Unique request id</param>
    /// <param name="unicodeCharacterHandlingPolicy">Unicode handling policy</param>
    public SmartSendMessage(string sender, string content, SmartSendMessageRecipient[] recipients, string dlrUrl, string customerData, DateTime? sendDateTime, TimeSpan? allowedSendTimeStart, TimeSpan? allowedSendTimeEnd, DayOfWeek[] allowedSendDays, string requestId, UnicodeCharacterHandlingPolicy unicodeCharacterHandlingPolicy)
    {
        Sender = sender;
        Content = content;
        Recipients = recipients;
        DlrUrl = dlrUrl;
        CustomerData = customerData;
        SendDateTime = sendDateTime;
        AllowedSendTimeStart = allowedSendTimeStart;
        AllowedSendTimeEnd = allowedSendTimeEnd;
        AllowedSendDays = allowedSendDays;
        RequestId = requestId;
        UnicodeCharacterHandlingPolicy = unicodeCharacterHandlingPolicy;
    }
}