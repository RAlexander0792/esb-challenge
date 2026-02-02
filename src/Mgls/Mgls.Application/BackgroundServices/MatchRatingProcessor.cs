using Mgls.Application.Factories;
using Mgls.Application.Services.RatingResolvers;
using Mgls.Domain.Entities;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Mgls.Application.BackgroundServices;
public sealed class MatchRatingProcessor : BackgroundService
{
    private readonly Channel<Match> _channel;
    private readonly ILogger<MatchRatingProcessor> _logger;
    private readonly IRatingResolverFactory _eloResolverFactory;
    private readonly ILeaderboardService _leaderboardService;
    private readonly ILeaderboardPlayerRatingAuditService _playerEloAuditService;
    private readonly ISortedLeaderboardService _sortedLeaderboardService;

    private readonly ConcurrentDictionary<Guid, IRatingResolver> _resolversByLeaderboard = new();

    public MatchRatingProcessor(
        IRatingResolverFactory eloResolverFactory,
        ILeaderboardService leaderboardService,
        ILeaderboardPlayerRatingAuditService playerEloAuditService,
        Channel<Match> channel,
        ISortedLeaderboardService sortedLeaderboardService,
        ILogger<MatchRatingProcessor> logger)
    {
        _eloResolverFactory = eloResolverFactory;
        _leaderboardService = leaderboardService;
        _playerEloAuditService = playerEloAuditService;
        _sortedLeaderboardService = sortedLeaderboardService;
        _channel = channel;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (var message in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                await ProcessMatch(message, stoppingToken);
            }
        }
        catch (OperationCanceledException ex) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogTrace(ex, "Channel requested closing");
        }
        catch (ChannelClosedException ex)
        {
            _logger.LogError(ex, "Channel closed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MatchRatingProcessor crashed.");
            throw;
        }
    }

    private async Task ProcessMatch(Match message, CancellationToken ct)
    {
        var resolver = await GetRatingResolver(message.LeaderboardId, ct);

        var playersIds = message.PlayerScores.Select(x => x.PlayerId).ToList();
        var leaderboardPlayers = await _leaderboardService.GetLeaderboardPlayers(message.LeaderboardId, playersIds, ct);

        var existingIds = leaderboardPlayers.Where(p => p != null).Select(p => p!.Id.PlayerId).ToHashSet();
        var nonRankedPlayers = playersIds.Where(id => !existingIds.Contains(id));

        foreach (var nonRankedPlayer in nonRankedPlayers)
        {
            var newRankedPlayer =
                await _leaderboardService.InitializeLeaderboardPlayer(message.LeaderboardId, nonRankedPlayer, ct);
            leaderboardPlayers.Add(newRankedPlayer);
        }

        var finalizedPlayers = leaderboardPlayers.Where(x => x != null).ToList();
        if (message.PlayerScores.Count != finalizedPlayers.Count)
        {
            _logger.LogWarning(
                "Skipping match {MatchId}: expected {Expected} players, got {Actual}.",
                message.Id, message.PlayerScores.Count, finalizedPlayers.Count);
            return;
        }

        var changes = resolver.CalculateRatingChangesByMatch(message, finalizedPlayers!);

        var ok = await _playerEloAuditService.BatchInsertPlayerRatingAudit(
            new CreateLeaderboardPlayerRatingAuditsDto { Payload = changes });

        if (!ok) return;

        foreach (var change in changes)
        {
            var rankedPlayer = finalizedPlayers!.First(x =>
                x!.Id == new LeaderboardPlayerId(change.Id.LeaderboardId, change.Id.PlayerId))!;

            await _leaderboardService.UpdateLeaderboardPlayerFromAudit(change, rankedPlayer, ct);
            await _sortedLeaderboardService.Update(message.LeaderboardId, change.Id.PlayerId, change.RatingAfter, ct);
        }
    }

    private async Task<IRatingResolver> GetRatingResolver(Guid leaderboardId, CancellationToken ct)
    {
        if (_resolversByLeaderboard.TryGetValue(leaderboardId, out var existing))
            return existing;

        var ratingRuleset = await _leaderboardService.GetLeaderboardRatingRuleset(leaderboardId);
        var created = _eloResolverFactory.CreateEloPlusResolver(ratingRuleset);

        return _resolversByLeaderboard.GetOrAdd(leaderboardId, created);
    }
}
