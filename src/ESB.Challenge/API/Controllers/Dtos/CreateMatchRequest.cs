
using ESBC.Domain.Entities;

namespace ESBC.API.Controllers.Dtos;

public class CreateMatchRequest
{
    public Guid RankedBoardId { get; set; }
    public List<MatchPlayerScore> PlayerScores { get; set; }
}
