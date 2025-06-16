namespace ArenaInteractive.Definitions;

using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Options class usually contains parameters that are required.
/// </summary>
public record Options
{
    /// <summary>
    /// PAT (Personal Access Token) for sending messages
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [Required]
    [PasswordPropertyText]
    public string PersonalAccessToken { get; set; }

    /// <summary>
    /// Optional, URL for Delivery reports
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [MaxLength(1000)]
    public string DlrUrl { get; set; }

    /// <summary>
    /// Optional, data to attach to the message event, used in reporting.
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [MaxLength(255)]
    public string CustomerData { get; set; }

    /// <summary>
    /// Optional, for scheduled messages
    /// </summary>
    /// <example>2025-04-22T10:45:23Z</example>
    public DateTime? SendDateTime { get; set; }

    /// <summary>
    /// Optional, for limiting time of day (UTC) when messages are allowed to be sent (start of range)
    /// </summary>
    /// <example>08:00:00</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string AllowedSendTimeStart { get; set; }

    /// <summary>
    /// Optional, for limiting time of day (UTC) when messages are allowed to be sent (end of range)
    /// </summary>
    /// <example>16:00:00</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string AllowedSendTimeEnd { get; set; }

    /// <summary>
    /// Optional, for limiting day(s) of week when messages are allowed to be sent
    /// </summary>
    public DayOfWeek[] AllowedSendDays { get; set; }

    /// <summary>
    /// Optional, to detect duplicate requests. If not set, a value will be generated.
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [MaxLength(1000)]
    public string RequestId { get; set; }

    /// <summary>
    /// Optional, for controlling Unicode character handling
    /// </summary>
    [DefaultValue(UnicodeCharacterHandlingPolicy.None)]
    public UnicodeCharacterHandlingPolicy UnicodeCharacterHandlingPolicy { get; set; }

    /// <summary>
    /// Throw exception if return value of request is not successful.
    /// </summary>
    [DefaultValue("true")]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Error message in case of thrown exception or task failure
    /// </summary>
    public string ErrorMessageOnFailure { get; set; }
}

/// <summary>
/// Enum for <see cref="Options.UnicodeCharacterHandlingPolicy"/>
/// </summary>
public enum UnicodeCharacterHandlingPolicy
{
    /// <summary>
    /// No special handling for Unicode characters
    /// </summary>
    None,

    /// <summary>
    /// Unicode characters detected in <see cref="Input.Content"/> will fail validation and message will not be sent
    /// </summary>
    Strict,

    /// <summary>
    /// Unicode characters will be replaced with closest non-Unicode character, or by whitespace.
    /// </summary>
    Replace,
}