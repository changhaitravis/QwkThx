using System.Text.Json.Serialization;

namespace QwkThx.Models;

/// <summary>
/// Payload posted to a Slack incoming webhook.
/// </summary>
public sealed class SlackPayload
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
