using Mgls.Domain.Services.Dtos;

namespace Mgls.Domain.Services;

public interface ISortedLeaderboardService
{
    Task<long?> GetRank(Guid leaderboardId, Guid playerId, CancellationToken ct); // 1-based
    Task Update(Guid leaderboardId, Guid playerId, int rating, CancellationToken ct);
    public Task<List<PlayerLeaderboardRankDto>> GetTopPlayerLeadboardStats(Guid leaderboardId, int skip, int take);
}
