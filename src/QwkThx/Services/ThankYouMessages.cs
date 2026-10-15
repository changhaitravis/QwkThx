namespace QwkThx.Services;

/// <summary>
/// Builds the Slack message text for a thank-you. Kept separate from the
/// HTTP plumbing so the wording is unit-testable.
/// </summary>
public static class ThankYouMessages
{
    /// <summary>Short thank-you posted from a pre-filled link (no comment).</summary>
    public static string QuickThankYou(string handler, string customer) =>
        $"{handler} received a thank you from {customer}";

    /// <summary>Thank-you posted from the form, including the customer's note.</summary>
    public static string ThankYouWithNote(string handler, string comment) =>
        $"{handler} received a thank you with the note:\n{comment}";
}
