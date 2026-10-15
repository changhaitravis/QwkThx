namespace QwkThx.Options;

/// <summary>
/// Incoming Slack webhook settings. Bind from the "Slack" configuration
/// section (appsettings.json, environment variables, or user secrets).
/// </summary>
public sealed class SlackOptions
{
    public const string SectionName = "Slack";

    /// <summary>
    /// Full incoming-webhook URL, e.g. https://hooks.slack.com/services/T.../B.../....
    /// Leave empty until configured; posts are skipped with a warning instead
    /// of failing the request.
    /// </summary>
    public string? WebhookUrl { get; set; }

    /// <summary>Display name Slack shows on the posted message.</summary>
    public string BotUsername { get; set; } = "QwkThx";

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(WebhookUrl) && !WebhookUrl.Contains("___", StringComparison.Ordinal);
}
