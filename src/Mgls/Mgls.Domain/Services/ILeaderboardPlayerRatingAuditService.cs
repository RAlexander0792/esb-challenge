using Mgls.Domain.Services.Dtos;

namespace Mgls.Domain.Services;

public interface ILeaderboardPlayerRatingAuditService
{
    Task<bool> BatchInsertPlayerRatingAudit(CreateLeaderboardPlayerRatingAuditsDto batchInsertPlayerRatingAudit);
}
