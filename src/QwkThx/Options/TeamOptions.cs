namespace QwkThx.Options;

/// <summary>
/// Team roster shown in the thank-you form dropdown. Bind from the "Team"
/// configuration section so the staff list no longer lives in code.
/// </summary>
public sealed class TeamOptions
{
    public const string SectionName = "Team";

    public List<TeamMember> Staff { get; set; } = new();
}

public sealed class TeamMember
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Role { get; set; }

    public override string ToString() => Username;
}
