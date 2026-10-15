using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QwkThx.Models;
using QwkThx.Options;
using QwkThx.Services;

namespace QwkThx.Controllers;

public class HomeController : Controller
{
    private readonly ISlackNotifier _slack;
    private readonly TeamOptions _team;

    public HomeController(ISlackNotifier slack, IOptions<TeamOptions> team)
    {
        _slack = slack;
        _team = team.Value;
    }

    /// <summary>
    /// Shows the thank-you form. When handler, ticket, and customer are all
    /// supplied as query parameters (e.g. from a ticket link), the thank-you
    /// is posted to Slack immediately without showing the form.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(string? handler, string? ticket, string? customer,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(handler)
            && !string.IsNullOrWhiteSpace(ticket)
            && !string.IsNullOrWhiteSpace(customer))
        {
            var result = await _slack.PostAsync(
                ThankYouMessages.QuickThankYou(handler, customer), cancellationToken);
            return result == SlackPostResult.Posted ? View("Success") : View("Error");
        }

        return View(new ThankYouForm
        {
            Handler = handler,
            Ticket = ticket,
            Customer = customer,
            Staff = _team.Staff,
        });
    }

    /// <summary>
    /// Posts the thank-you form to Slack.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ThankYouForm form, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(form.Handler))
        {
            ModelState.AddModelError(nameof(form.Handler), "Choose who deserves the commendation.");
        }

        if (!ModelState.IsValid)
        {
            form.Staff = _team.Staff;
            return View(form);
        }

        var result = await _slack.PostAsync(
            ThankYouMessages.ThankYouWithNote(form.Handler!, form.Comment ?? string.Empty),
            cancellationToken);

        return result == SlackPostResult.Posted ? View("Success") : View("Error");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
