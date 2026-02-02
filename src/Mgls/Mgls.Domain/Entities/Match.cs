namespace Mgls.Domain.Entities;

public class Match
{
    public Guid Id { get; set; }
    public Guid LeaderboardId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset EndedAt { get; set; }
    public List<MatchPlayerScore> PlayerScores { get; set; }
}
