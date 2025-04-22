namespace ArenaInteractive.Definitions;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Input class usually contains parameters that are required.
/// </summary>
public class Input
{
    /// <summary>
    /// Your CustomerId
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [Required]
    public Guid CustomerId { get; set; }

    /// <summary>
    /// ServiceId for service to use to send the message
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [Required]
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Sender address or name
    /// </summary>
    /// <example>+358101001234</example>
    /// <example>SmsSender</example>
    /// <example>16010</example>
    [DisplayFormat(DataFormatString = "Text")]
    [Required]
    [MaxLength(20)]
    public string Sender { get; set; }

    /// <summary>
    /// Message content. May contain variable placeholders like $(variable), which can be used by <see cref="Recipient.Personalization"/>
    /// </summary>
    /// <example>Hello world</example>
    [DisplayFormat(DataFormatString = "Text")]
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }

    /// <summary>
    /// Message recipient phone numbers, preferably in E.164 format
    /// </summary>
    [Required]
    public Recipient[] Recipients { get; set; }
}

/// <summary>
/// Recipient
/// </summary>
public record Recipient
{
    /// <summary>
    /// Recipient address (phone number), preferably in E.164 format
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    /// Optional, recipient-specific personalization
    /// </summary>
    public Dictionary<string, string> Personalization { get; set; }
}