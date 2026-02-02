using Mgls.Domain.Entities;

namespace Mgls.Domain.Services.Dtos;

public class CreateMatchDto
{
    public Guid LeaderboardId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset EndedAt { get; set; }
    public List<MatchPlayerScore> PlayerScores { get; set; }
}
