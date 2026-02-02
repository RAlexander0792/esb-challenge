using Mgls.Domain.Entities;
using Mgls.Domain.Services.Dtos;

namespace Mgls.Domain.Services;

public interface IMatchService
{
    Task<Match> Create(CreateMatchDto match);
    Task<List<Match>> GetByLeaderboard(Guid leaderboardId, int limit, CancellationToken ct);
    Task<List<Match>> GetByPlayer(Guid playerId, CancellationToken ct, Guid? leaderboardId = null, int limit = 100);
}
