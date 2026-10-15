# QwkThx

Have your customers fill out a form and post thank-yous to Slack — instead of re-opening a customer service ticket.

This is a port of the original 2016 ASP.NET MVC 5 app to **ASP.NET Core on .NET 10**, with a few long-standing issues fixed along the way.

## How it works

- `GET /` — shows the thank-you form (pick a teammate, write a note).
- `GET /?handler=alice&ticket=123&customer=bob` — posts a short thank-you straight to Slack without showing the form (handy for linking from a ticket).
- `POST /` — submits the form; the note is posted to Slack.

A successful post renders a confirmation page; a failed or unconfigured post renders an error page.

## Configuration

Set your Slack incoming webhook URL (from your Slack app's *Incoming Webhooks* page):

```json
{
  "Slack": {
    "WebhookUrl": "https://hooks.slack.com/services/T.../B.../...",
    "BotUsername": "QwkThx"
  }
}
```

Prefer user secrets in development and the `Slack__WebhookUrl` environment variable in production over committing the URL. If no webhook is configured, posts are skipped with a warning and the error page is shown.

The team roster lives in configuration too, under `"Team": { "Staff": [...] }`.

## Run it

```bash
dotnet run --project src/QwkThx
```

Then open the URL printed to the console (e.g. http://localhost:5000).

## Tests

```bash
dotnet test
```

## What changed in the modernization

- Retargeted from .NET Framework 4.5 / ASP.NET MVC 5 to .NET 10 / ASP.NET Core MVC (SDK-style project, nullable reference types, implicit usings).
- **Fixed the success detection**: the old code checked `IsCompleted` on a never-awaited `Task`, so it *always* showed the success page even when the Slack post failed. Posts are now awaited and `IsSuccessStatusCode` is checked.
- Slack webhook URL moved from a hardcoded placeholder to configuration (`Slack:WebhookUrl`).
- `IHttpClientFactory` typed client replaces `new HttpClient()` per request.
- Public fields became properties; the staff list moved from hardcoded C# to configuration.
- Removed the unused ReactJS.NET / ClearScript V8 baggage (including the checked-in native DLLs), `packages.config`, and the old `Web.config`/`Global.asax` pipeline.
