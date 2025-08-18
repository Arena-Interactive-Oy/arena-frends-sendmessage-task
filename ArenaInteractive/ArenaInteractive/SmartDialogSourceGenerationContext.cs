namespace ArenaInteractive.SmartDialog.SendSmartMessage;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DTOs;

/// <summary>
/// Source generation context
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(SmartSendMessage))]
[JsonSerializable(typeof(SmartSendMessageRecipient))]
[JsonSerializable(typeof(SendResponse))]
[ExcludeFromCodeCoverage]
public partial class SmartDialogSourceGenerationContext : JsonSerializerContext
{
}