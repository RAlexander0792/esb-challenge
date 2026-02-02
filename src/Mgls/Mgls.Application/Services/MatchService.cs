using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using System.Threading.Channels;

namespace Mgls.Application.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly Channel<Match> _channel;

    public MatchService(IMatchRepository matchRepository, Channel<Match> channel)
    {
        _matchRepository = matchRepository;
        _channel = channel;
    }

    public async Task<Match> Create(CreateMatchDto match)
    {
        var newMatch = new Match()
        {
            CreatedAt = match.CreatedAt,
            StartedAt = match.StartedAt,
            LeaderboardId = match.LeaderboardId,
            PlayerScores = match.PlayerScores,
            EndedAt = match.EndedAt
        };

        await _matchRepository.InsertAsync(newMatch, new CancellationToken());

        await _channel.Writer.WriteAsync(newMatch);

        return newMatch;
    }

    public async Task<List<Match>> GetByLeaderboard(Guid leaderboardId, int limit, CancellationToken ct)
    {
        return await _matchRepository.GetByLeaderboard(leaderboardId, limit, ct);
    }

    public async Task<List<Match>> GetByPlayer(Guid playerId, CancellationToken ct, Guid? leaderboardId = null, int limit = 100)
    {
        return await _matchRepository.GetByPlayer(playerId, leaderboardId, limit, ct);
    }
}
