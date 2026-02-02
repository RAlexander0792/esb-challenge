
using Mgls.Domain.Entities;

namespace Mgls.API.Controllers.Dtos;

public class CreateMatchRequest
{
    public Guid LeaderboardId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public List<MatchPlayerScore> PlayerScores { get; set; }
}
