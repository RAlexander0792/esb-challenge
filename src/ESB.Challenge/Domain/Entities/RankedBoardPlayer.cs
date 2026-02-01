namespace ESBC.Domain.Entities;

public class RankedBoardPlayer
{
    public RankedBoardPlayerId Id { get; set; }
    public int ELO { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public Guid LastAppliedMatchId { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}
public sealed record RankedBoardPlayerId(Guid RankedBoardId, Guid PlayerId);