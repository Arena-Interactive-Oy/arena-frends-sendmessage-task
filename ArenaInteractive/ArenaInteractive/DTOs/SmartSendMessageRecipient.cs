namespace ArenaInteractive.DTOs;

using System.Collections.Generic;

/// <summary>
/// Part of the request object type for Smart Send Messages API
/// </summary>
public class SmartSendMessageRecipient
{
    /// <summary>
    /// Gets or sets recipient address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets personalization
    /// </summary>
    public Dictionary<string, string> Personalization { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartSendMessageRecipient"/> class.
    /// </summary>
    /// <param name="address">Recipient address</param>
    /// <param name="personalization">Personalization</param>
    public SmartSendMessageRecipient(string address, Dictionary<string, string> personalization)
    {
        Address = address;
        Personalization = personalization;
    }
}