namespace Mgls.Domain.Entities;

public class LeaderboardPlayerRatingAudit
{
    public LeaderboardPlayerRatingAuditId Id { get; set; }
    public int RatingBefore { get; set; }
    public int RatingAfter { get; set; }
    public double ExpectedTotal { get; set; }
    public double ActualTotal { get; set; }
    public double DeltaTotal { get; set; }
    public double Score { get; set; }
    public RatingRuleset RulesetSnapshot { get; set; } = default!;
    public ICollection<LeaderboardPlayerRatingVersusAudit> Versus { get; set; } = [];
}
public record LeaderboardPlayerRatingAuditId(Guid LeaderboardId, Guid MatchId, Guid PlayerId);

