namespace ArenaInteractive.Definitions;

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