using Mgls.Domain.Entities;

namespace Mgls.Domain.Services.Dtos;

public class CreateLeaderboardPlayerRatingAuditsDto
{
    public List<LeaderboardPlayerRatingAudit> Payload { get; set; }
}
