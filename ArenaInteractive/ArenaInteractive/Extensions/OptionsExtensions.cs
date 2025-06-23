namespace ArenaInteractive.Extensions;

using System;
using Definitions;
using System.Globalization;

internal static class OptionsExtensions
{
    internal static string Validate(this Options options)
    {
        if (string.IsNullOrWhiteSpace(options.PersonalAccessToken))
        {
            return $"{nameof(Options.PersonalAccessToken)} is required";
        }

        if (options.AllowedSendDays is { Length: 0 })
        {
            options.AllowedSendDays = Constants.AllDaysOfWeek;
        }

        if (TimeSpan.TryParse(options.AllowedSendTimeStart, CultureInfo.InvariantCulture, out var allowedTimeStart) &&
            TimeSpan.TryParse(options.AllowedSendTimeEnd, CultureInfo.InvariantCulture, out var allowedTimeEnd) &&
            allowedTimeStart > allowedTimeEnd)
        {
            return $"{nameof(Options.AllowedSendTimeStart)} must smaller than {nameof(Options.AllowedSendTimeEnd)}";
        }

        if (!string.IsNullOrWhiteSpace(options.CustomerData) && options.CustomerData.Length > 255)
        {
            return $"{nameof(Options.CustomerData)} must be max 255 characters";
        }

        if (!string.IsNullOrWhiteSpace(options.DlrUrl) && !Uri.TryCreate(options.DlrUrl, UriKind.Absolute, out _))
        {
            return $"{nameof(Options.DlrUrl)} must be a valid absolute URI";
        }

        if (options.RequestId is { Length: > 1000 })
        {
            return $"{nameof(Options.RequestId)} must be max 1000 characters";
        }

        var limit = DateTime.UtcNow.AddMinutes(5);
        if (options.SendDateTime != null && options.SendDateTime.Value.ToUniversalTime() < limit)
        {
            return $"{nameof(Options.SendDateTime)} must be at least 5 minutes in the future";
        }

        return null;
    }
}