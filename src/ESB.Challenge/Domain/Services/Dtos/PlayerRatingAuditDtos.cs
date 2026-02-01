using ESBC.Domain.Entities;

namespace ESBC.Domain.Services.Dtos;

public class CreatePlayerEloAuditDto
{
    public List<RankedBoardPlayerRatingAudit> Payload { get; set; }
}
