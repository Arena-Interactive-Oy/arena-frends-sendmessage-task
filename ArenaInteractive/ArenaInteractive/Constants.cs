namespace ArenaInteractive;

using System.Net;

/// <summary>
/// Contains common constants
/// </summary>
public static class Constants
{
    public static readonly HttpStatusCode[] HandledStatusCodes = [HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.TooManyRequests];
}