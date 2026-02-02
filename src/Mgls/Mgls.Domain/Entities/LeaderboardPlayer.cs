namespace Mgls.Domain.Entities;

public class LeaderboardPlayer
{
    public LeaderboardPlayerId Id { get; set; }
    public int Rating { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Streak { get; set; }
    public Guid LastAppliedMatchId { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; }
}
public sealed record LeaderboardPlayerId(Guid LeaderboardId, Guid PlayerId);