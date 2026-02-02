using Mgls.Domain.Entities;

namespace Mgls.Domain.Repositories;

public interface ILeaderboardRepository
{
    Task<Leaderboard?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Leaderboard>> GetLeaderboards(CancellationToken ct);
    Task InsertAsync(Leaderboard doc, CancellationToken ct);
}
