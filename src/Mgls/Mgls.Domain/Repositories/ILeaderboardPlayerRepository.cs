using Mgls.Domain.Entities;

namespace Mgls.Domain.Repositories;

public interface ILeaderboardPlayerRepository
{
    Task<LeaderboardPlayer?> GetByLeaderboardIdAsync(Guid id, Guid playerId, CancellationToken ct);
    Task<List<LeaderboardPlayer>> GetTopPlayers(Guid leaderboardId, int limit, CancellationToken ct);
    Task UpdateLeaderboardPlayer(LeaderboardPlayer doc, CancellationToken ct);
    Task InsertAsync(LeaderboardPlayer doc, CancellationToken ct);
    Task<List<LeaderboardPlayer>> GetByPlayerId(Guid playerId, CancellationToken ct);
    Task<List<LeaderboardPlayer>> GetByPlayerIds(Guid leaderboardId, IEnumerable<Guid> pagePlayersIds);
}
