using QwkThx.Options;

namespace QwkThx.Models;

/// <summary>
/// Backing model for the thank-you form.
/// </summary>
public sealed class ThankYouForm
{
    public string? Handler { get; set; }
    public string? Comment { get; set; }
    public string? Ticket { get; set; }
    public string? Customer { get; set; }

    public List<TeamMember> Staff { get; set; } = new();
}
