namespace ArenaInteractive.Extensions;

using Definitions;

internal static class InputExtensions
{
    internal static string Validate(this Input input)
    {
        if (string.IsNullOrWhiteSpace(input.Content) || input.Content.Length > 1000)
        {
            return $"{nameof(Input.Content)} is required and must be max 1000 characters";
        }

        if (string.IsNullOrWhiteSpace(input.Sender) || input.Sender.Length > 20)
        {
            return $"{nameof(Input.Sender)} is required and must be max 20 characters";
        }

        if (input.Recipients.Length == 0)
        {
            return $"{nameof(Input.Recipients)} have at least 1 recipient";
        }

        for (var i = 0; i < input.Recipients.Length; i++)
        {
            var recipient = input.Recipients[i];
            if (string.IsNullOrWhiteSpace(recipient.Address) || recipient.Address.Length > 20)
            {
                return $"{nameof(Input.Recipients)}[{i}.{nameof(Recipient.Address)} is required and must be max 20 characters";
            }
        }

        return null;
    }
}