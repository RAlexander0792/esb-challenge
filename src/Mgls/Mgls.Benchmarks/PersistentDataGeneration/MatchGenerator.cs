using Mgls.API.Controllers.Dtos;
using Mgls.Benchmarks.EntityBuilders;
using Mgls.Domain.Entities;
using Mgls.Domain.Services;
using Mgls.UtilitiesCli.ApiProviders;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System.Net;

namespace Mgls.UtilitiesCli.PersistentDataGeneration;

public class MatchGenerator
{
    private readonly IPlayerService _playerService;
    private readonly IMatchProvider _matchProvider;
    private MatchBuilder _matchBuilder { get; set; }
    public MatchGenerator()
    {
        _playerService = DependencyManager.ServiceProvider.GetRequiredService<IPlayerService>();
        _matchProvider = DependencyManager.ServiceProvider.GetRequiredService<IMatchProvider>();

        
    }

    public async Task Load(int seed, Guid leaderboardId, int howManyPlayers, int playerPerMatch, long scoreFrom, long scoreTo)
    {
        var players = await _playerService.GetPlayers(howManyPlayers, CancellationToken.None);
        var rankedPlayers = players
            .Select(x => new LeaderboardPlayer() { Id = new LeaderboardPlayerId(leaderboardId, x.Id) });


        _matchBuilder = new MatchBuilder(seed);

        _matchBuilder.ForLeaderboard(leaderboardId);
        _matchBuilder.WithPlayersPerMatch(playerPerMatch);
        _matchBuilder.WithPlayers(rankedPlayers.ToList());
        _matchBuilder.WithScoreRange(scoreFrom, scoreTo);
    }

    public async Task<int> Generate()
    {
        var match = _matchBuilder.BuildOne();
        var response = await _matchProvider.PostMatch(new CreateMatchRequest() { LeaderboardId = match.LeaderboardId, PlayerScores = match.PlayerScores });
        var code = ((int)response.StatusCode);
        response.Dispose();
        return code;
    }
}
