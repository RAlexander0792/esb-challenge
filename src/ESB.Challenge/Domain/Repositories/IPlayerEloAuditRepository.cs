using ESBC.Domain.Entities;

namespace ESBC.Domain.Repositories;

public interface IPlayerEloAuditRepository
{
    Task<RankedBoardPlayerRatingAudit?> GetByIdAsync(RankedBoardPlayerRatingAuditId id, CancellationToken ct);
    Task InsertAsync(RankedBoardPlayerRatingAudit doc, CancellationToken ct);
}
