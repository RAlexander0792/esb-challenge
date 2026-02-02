using Mgls.Domain.Entities;
using Mgls.Domain.Services.Dtos;

namespace Mgls.Domain.Services;

public interface ILeaderboardService
{
    Task<Leaderboard> CreateLeaderboard(CreateLeaderboardDto leaderboard, CancellationToken ct);
    Task<Leaderboard?> GetById(Guid leaderboardId, CancellationToken ct);
    Task<List<Match>> GetLeaderboardMatchHistory(Guid leaderboardId, int limit, CancellationToken ct);
    Task<List<LeaderboardPlayer>> GetLeaderboardPlayers(Guid leaderboardId, IEnumerable<Guid> playerIds, CancellationToken ct);
    Task<RatingRuleset> GetLeaderboardRatingRuleset(Guid leaderboardId);
    Task<List<Leaderboard>> GetLeaderboards(CancellationToken ct);
    Task<List<LeaderboardPlayer>> GetTopPlayers(Guid leaderboardId, int limit, CancellationToken ct);
    Task<LeaderboardPlayer> InitializeLeaderboardPlayer(Guid leaderboardId, Guid playerId, CancellationToken ct);
    Task UpdateLeaderboardPlayerFromAudit(LeaderboardPlayerRatingAudit record, LeaderboardPlayer leaderboardPlayer, CancellationToken ct);
}
