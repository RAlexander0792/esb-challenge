using ESBC.Domain.Services.Dtos;

namespace ESBC.Domain.Services;

public interface IPlayerEloAuditService
{
    Task<bool> BatchInsertPlayerRatingAudit(CreatePlayerEloAuditDto batchInsertPlayerRatingAudit);
}
