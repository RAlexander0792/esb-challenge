using ESBC.Domain.Entities;

namespace ESBC.Domain.Services.Dtos;

public class CreateMatchDto
{
    public Guid RankedBoardId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset EndedAt { get; set; }
    public List<MatchPlayerScore> PlayerScores { get; set; }
}
