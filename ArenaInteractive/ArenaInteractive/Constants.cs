namespace ArenaInteractive;
using System;
using System.Net;

/// <summary>
/// Contains common constants
/// </summary>
public static class Constants
{
    public static readonly HttpStatusCode[] HandledStatusCodes = new HttpStatusCode[] { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.TooManyRequests };

    public static readonly DayOfWeek[] AllDaysOfWeek = new DayOfWeek[]
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Saturday,
    };
}