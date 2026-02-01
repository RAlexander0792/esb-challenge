namespace ESBC.Domain.Entities;

public class Match
{
    public Guid Id { get; set; }
    public Guid RankedBoardId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset EndedAt { get; set; }
    public List<MatchPlayerScore> PlayerScores { get; set; }
}
