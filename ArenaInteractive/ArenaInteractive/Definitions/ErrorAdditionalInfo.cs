namespace ArenaInteractive.Definitions;

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