using Mgls.Domain.Entities;

namespace Mgls.Domain.Repositories;

public interface IMatchRepository
{
    public Task<Match?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Match>> GetByLeaderboard(Guid leaderboardId, int limit, CancellationToken ct);
    Task<List<Match>> GetByPlayer(Guid playerId, Guid? leaderboardId, int limit, CancellationToken ct);
    public Task InsertAsync(Match doc, CancellationToken ct);
}
