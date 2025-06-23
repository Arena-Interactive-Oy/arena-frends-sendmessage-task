namespace ArenaInteractive;

using DTOs;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

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