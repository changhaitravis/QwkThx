using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QwkThx.Options;
using QwkThx.Services;
using Xunit;

namespace QwkThx.Tests;

public sealed class ThankYouMessagesTests
{
    [Fact]
    public void QuickThankYou_FormatsHandlerAndCustomer()
    {
        Assert.Equal("alice received a thank you from bob",
            ThankYouMessages.QuickThankYou("alice", "bob"));
    }

    [Fact]
    public void ThankYouWithNote_IncludesHandlerAndComment()
    {
        Assert.Equal("alice received a thank you with the note:\nYou rock!",
            ThankYouMessages.ThankYouWithNote("alice", "You rock!"));
    }
}

public sealed class SlackNotifierTests
{
    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _respond;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) => _respond = respond;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken) => Task.FromResult(_respond(request));
    }

    private static SlackNotifier CreateNotifier(SlackOptions options, HttpMessageHandler? handler = null)
    {
        var http = handler is null ? new HttpClient() : new HttpClient(handler);
        return new SlackNotifier(http, Microsoft.Extensions.Options.Options.Create(options), NullLogger<SlackNotifier>.Instance);
    }

    [Fact]
    public async Task PostAsync_WithoutWebhookUrl_ReturnsNotConfigured()
    {
        var notifier = CreateNotifier(new SlackOptions { WebhookUrl = "" });

        Assert.Equal(SlackPostResult.NotConfigured, await notifier.PostAsync("hi"));
    }

    [Fact]
    public async Task PostAsync_WithPlaceholderUrl_ReturnsNotConfigured()
    {
        var notifier = CreateNotifier(new SlackOptions
        {
            WebhookUrl = "https://hooks.slack.com/services/___/___/___"
        });

        Assert.Equal(SlackPostResult.NotConfigured, await notifier.PostAsync("hi"));
    }

    [Fact]
    public async Task PostAsync_OnSuccess_ReturnsPosted_AndSendsPayload()
    {
        string? body = null;
        var handler = new StubHandler(req =>
        {
            body = req.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var notifier = CreateNotifier(
            new SlackOptions { WebhookUrl = "https://hooks.slack.com/services/T/B/x", BotUsername = "QwkThx" },
            handler);

        var result = await notifier.PostAsync("alice received a thank you from bob");

        Assert.Equal(SlackPostResult.Posted, result);
        Assert.Contains("\"username\":\"QwkThx\"", body);
        Assert.Contains("alice received a thank you from bob", body);
    }

    [Fact]
    public async Task PostAsync_OnServerError_ReturnsFailed()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var notifier = CreateNotifier(
            new SlackOptions { WebhookUrl = "https://hooks.slack.com/services/T/B/x" },
            handler);

        Assert.Equal(SlackPostResult.Failed, await notifier.PostAsync("hi"));
    }

    [Fact]
    public async Task PostAsync_OnTransportError_ReturnsFailed()
    {
        var handler = new StubHandler(_ => throw new HttpRequestException("boom"));
        var notifier = CreateNotifier(
            new SlackOptions { WebhookUrl = "https://hooks.slack.com/services/T/B/x" },
            handler);

        Assert.Equal(SlackPostResult.Failed, await notifier.PostAsync("hi"));
    }
}
