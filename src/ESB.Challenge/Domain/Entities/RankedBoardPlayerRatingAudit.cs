namespace ESBC.Domain.Entities;

public record RankedBoardPlayerRatingAuditId(Guid RankedBoardId, Guid MatchId, Guid PlayerId);
public class RankedBoardPlayerRatingAudit
{
    public RankedBoardPlayerRatingAuditId Id { get; set; }
    public int EloBefore { get; set; }
    public int EloAfter { get; set; }
    public double ExpectedTotal { get; set; }
    public double ActualTotal { get; set; }
    public double DeltaTotal { get; set; }
    public int RulesetVersion { get; set; }
    public EloRulesetSnapshot RulesetSnapshot { get; set; } = default!;
    public ICollection<PlayerEloAuditVersus> Versus { get; set; } = [];

}
