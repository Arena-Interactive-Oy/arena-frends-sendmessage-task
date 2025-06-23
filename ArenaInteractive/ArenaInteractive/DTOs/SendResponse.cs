namespace ArenaInteractive.DTOs;

using System;

/// <summary>
/// Response object type for Smart Send Messages API
/// </summary>
public class SendResponse
{
    /// <summary>
    /// Gets or sets Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets RecipientCount
    /// </summary>
    public int? RecipientCount { get; set; }

    /// <summary>
    /// Gets or sets MessagePartCount
    /// </summary>
    public int? MessagePartCount { get; set; }

    /// <summary>
    /// Gets or sets SendDateTimeEstimate
    /// </summary>
    public DateTime? SendDateTimeEstimate { get; set; }

    /// <summary>
    /// Gets or sets Warnings array
    /// </summary>
    public string[] Warnings { get; set; }

    /// <summary>
    /// Gets or sets Error
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets a value indicating whether request to send was successful or not
    /// </summary>
    public bool Successful => !string.IsNullOrWhiteSpace(Id) && RecipientCount > 0;
}