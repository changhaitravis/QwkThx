namespace QwkThx.Services;

public enum SlackPostResult
{
    Posted,
    Failed,
    NotConfigured,
}

public interface ISlackNotifier
{
    /// <summary>
    /// Posts <paramref name="text"/> to the configured Slack incoming webhook.
    /// </summary>
    Task<SlackPostResult> PostAsync(string text, CancellationToken cancellationToken = default);
}
