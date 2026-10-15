using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using QwkThx.Models;
using QwkThx.Options;

namespace QwkThx.Services;

/// <summary>
/// Posts thank-you messages to a Slack incoming webhook via a typed
/// <see cref="HttpClient"/> (registered with IHttpClientFactory).
/// </summary>
public sealed class SlackNotifier : ISlackNotifier
{
    private readonly HttpClient _http;
    private readonly SlackOptions _options;
    private readonly ILogger<SlackNotifier> _logger;

    public SlackNotifier(HttpClient http, IOptions<SlackOptions> options, ILogger<SlackNotifier> logger)
    {
        _http = http;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<SlackPostResult> PostAsync(string text, CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogWarning("Slack webhook URL is not configured; skipping post.");
            return SlackPostResult.NotConfigured;
        }

        var payload = new SlackPayload
        {
            Username = _options.BotUsername,
            Text = text,
        };

        try
        {
            using var response = await _http.PostAsJsonAsync(_options.WebhookUrl, payload, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return SlackPostResult.Posted;
            }

            _logger.LogWarning("Slack post failed with status {StatusCode}.", response.StatusCode);
            return SlackPostResult.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Slack post threw an exception.");
            return SlackPostResult.Failed;
        }
    }
}
