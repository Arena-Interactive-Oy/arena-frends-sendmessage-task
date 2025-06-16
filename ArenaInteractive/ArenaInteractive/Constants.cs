namespace ArenaInteractive;
using System;
using System.Net;

/// <summary>
/// Contains common constants
/// </summary>
public static class Constants
{
    /// <summary>
    /// Number of retries to attempt when response is not one of the handled status codes
    /// </summary>
    public const int MaxRetries = 10;

    /// <summary>
    /// HTTP status codes that are expected and can be handled
    /// </summary>
    public static readonly HttpStatusCode[] HandledStatusCodes = new HttpStatusCode[] { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.TooManyRequests };

    /// <summary>
    /// Convenience field for an array of all days of week
    /// </summary>
    public static readonly DayOfWeek[] AllDaysOfWeek = new DayOfWeek[]
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Saturday,
    };
}