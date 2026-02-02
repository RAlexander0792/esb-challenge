using Mgls.Domain.Entities;

namespace Mgls.Domain.Repositories;

public interface ILeaderboardPlayerRatingAuditRepository
{
    Task<LeaderboardPlayerRatingAudit?> GetByIdAsync(LeaderboardPlayerRatingAuditId id, CancellationToken ct);
    Task InsertAsync(LeaderboardPlayerRatingAudit doc, CancellationToken ct);
}
