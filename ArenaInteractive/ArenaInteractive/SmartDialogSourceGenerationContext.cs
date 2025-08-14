using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ArenaInteractive.SmartDialog.SendSmartMessage.DTOs;

namespace ArenaInteractive.SmartDialog.SendSmartMessage;

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