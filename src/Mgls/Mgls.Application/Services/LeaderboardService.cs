using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;

namespace Mgls.Application.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardRepository;
    private readonly ILeaderboardPlayerRepository _leaderboardPlayerRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IRatingRulesetRepository _ratingRulesetRepository;

    public LeaderboardService(ILeaderboardRepository leaderboardRepository,
                              ILeaderboardPlayerRepository leaderboardPlayerRepository,
                              IPlayerRepository playerRepository,
                              IMatchRepository matchRepository,
                              IRatingRulesetRepository ratingRulesetRepository)
    {
        _leaderboardRepository = leaderboardRepository;
        _leaderboardPlayerRepository = leaderboardPlayerRepository;
        _playerRepository = playerRepository;
        _matchRepository = matchRepository;
        _ratingRulesetRepository = ratingRulesetRepository;
    }

    public async Task<Leaderboard> CreateLeaderboard(CreateLeaderboardDto leaderboard, CancellationToken ct)
    {
        if (!await _ratingRulesetRepository.Exists(leaderboard.RatingRulesetId))
        {
            throw new ArgumentException("Invalid Rating Ruleset");
        }
        var newLeaderboard = new Leaderboard()
        {
            Name = leaderboard.Name,
            Description = leaderboard.Description,
            RatingRulesetId = leaderboard.RatingRulesetId,
        };

        await _leaderboardRepository.InsertAsync(newLeaderboard, ct);

        return newLeaderboard;
    }

    public async Task<Leaderboard?> GetById(Guid leaderboardId, CancellationToken ct)
    {
        return await _leaderboardRepository.GetByIdAsync(leaderboardId, ct);
    }

    public async Task<List<Match>> GetLeaderboardMatchHistory(Guid leaderboardId, int limit, CancellationToken ct)
    {
        return await _matchRepository.GetByLeaderboard(leaderboardId, limit, ct);
    }

    public async Task<List<LeaderboardPlayer>> GetLeaderboardPlayers(Guid leaderboardId, IEnumerable<Guid> playerIds, CancellationToken ct)
    {
        var players = new List<LeaderboardPlayer>();

        foreach (var playerId in playerIds)
        {
            var playerDb = await _leaderboardPlayerRepository.GetByLeaderboardIdAsync(leaderboardId, playerId, ct);
            players.Add(playerDb);
        }

        return players;
    }

    public async Task<RatingRuleset> GetLeaderboardRatingRuleset(Guid leaderboardId)
    {
        var leaderboard = await _leaderboardRepository.GetByIdAsync(leaderboardId, CancellationToken.None);

        var ratingRuleset = await _ratingRulesetRepository.GetByIdAsync(leaderboard.RatingRulesetId, CancellationToken.None);

        return ratingRuleset;
    }

    public async Task<List<Leaderboard>> GetLeaderboards(CancellationToken ct)
    {
        return await _leaderboardRepository.GetLeaderboards(ct);
    }

    public Task<List<LeaderboardPlayer>> GetTopPlayers(Guid leaderboardId, int limit, CancellationToken ct)
    {
        return _leaderboardPlayerRepository.GetTopPlayers(leaderboardId, limit, ct);
    }

    public async Task<LeaderboardPlayer> InitializeLeaderboardPlayer(Guid leaderboardId, Guid nonLeaderboardPlayer, CancellationToken ct)
    {
        if ((await _playerRepository.GetByIdAsync(nonLeaderboardPlayer, ct)) != null
           && await Exists(leaderboardId, ct))
        {
            LeaderboardPlayer? newLeaderboardPlayer = new LeaderboardPlayer()
            {
                Rating = 1500,
                Losses = 0,
                Wins = 0,
                LastAppliedMatchId = Guid.Empty,
                Id = new LeaderboardPlayerId(leaderboardId, nonLeaderboardPlayer),
                UpdatedAt = DateTime.UtcNow,
                Streak = 0,
                Version = 0
            };
            
            await _leaderboardPlayerRepository.InsertAsync(newLeaderboardPlayer, ct);
            return newLeaderboardPlayer;
        }
        return null;
    }

    public async Task UpdateLeaderboardPlayerFromAudit(LeaderboardPlayerRatingAudit record, LeaderboardPlayer leaderboardPlayer, CancellationToken ct)
    {
        leaderboardPlayer.Rating = record.RatingAfter;
        leaderboardPlayer.LastAppliedMatchId = record.Id.MatchId;
        leaderboardPlayer.Version++;

        if (record.DeltaTotal > 0)
        {
            leaderboardPlayer.Wins++;
            leaderboardPlayer.Streak = leaderboardPlayer.Streak > 0 ? leaderboardPlayer.Streak + 1 : 1;
        }
        else
        {
            leaderboardPlayer.Losses++;
            leaderboardPlayer.Streak = leaderboardPlayer.Streak < 0 ? leaderboardPlayer.Streak - 1 : -1;
        }    



        await _leaderboardPlayerRepository.UpdateLeaderboardPlayer(leaderboardPlayer, ct);
    }

    private async Task<bool> Exists(Guid leaderboardId, CancellationToken ct)
    {
        return await _leaderboardRepository.GetByIdAsync(leaderboardId, ct) != null;
    }
}
