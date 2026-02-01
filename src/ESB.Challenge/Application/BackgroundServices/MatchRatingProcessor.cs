using ESBC.Application.Factories;
using ESBC.Domain.Entities;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace ESBC.Application.BackgroundServices;

public class MatchRatingProcessor : IHostedService
{
    Task channelConsumer;
    private readonly Channel<Match> _channel;
    private readonly ILogger<MatchRatingProcessor> _logger;
    private readonly IEloResolverFactory _eloResolverFactory;
    private readonly IRankedBoardService _rankedBoardService;
    private readonly IPlayerEloAuditService _playerEloAuditService;
    public MatchRatingProcessor(IEloResolverFactory eloResolverFactory,
                                IRankedBoardService rankedBoardService,
                                IPlayerEloAuditService playerEloAuditService,
                                Channel<Match> channel)
    {
        _eloResolverFactory = eloResolverFactory;
        _rankedBoardService = rankedBoardService;
        _playerEloAuditService = playerEloAuditService;
        _channel = channel;
    }
    public async Task StartAsync(CancellationToken ct)
    {
        var resolver = _eloResolverFactory.CreateClassicResolver(new EloRulesetSnapshot() { K = 30, Tau = 10, Algorithm = "EloPlus" });
        channelConsumer = Task.Run( async () =>
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var message = await _channel.Reader.ReadAsync(ct);
                    var playersIds = message.PlayerScores.Select(x => x.PlayerId);
                    var rankedBoardPlayers = await _rankedBoardService.GetRankedBoardPlayers(message.RankedBoardId, playersIds, ct);
                    var nonRankedPlayers = playersIds.Except(rankedBoardPlayers.Where(c => c != null).Select(c => c.Id.PlayerId));
                    foreach (var nonRankedPlayer in nonRankedPlayers)
                    {
                        var newRankedPlayer = await _rankedBoardService.InitializeRankedPlayer(message.RankedBoardId, nonRankedPlayer);
                        rankedBoardPlayers.Add(newRankedPlayer);
                    }
                    rankedBoardPlayers = rankedBoardPlayers.Where(x => x != null).ToList();
                    if (message.PlayerScores.Count == rankedBoardPlayers.Count)
                    {
                        var changes = resolver.CalculateRatingChangesByMatch(message, rankedBoardPlayers);
                        var result = await _playerEloAuditService
                            .BatchInsertPlayerRatingAudit(new CreatePlayerEloAuditDto { Payload = changes });

                        if (result)
                        {
                            foreach(var change in changes)
                            {
                                var rankedPlayer = rankedBoardPlayers.First(x => x.Id == new RankedBoardPlayerId(change.Id.RankedBoardId, change.Id.PlayerId));
                                await _rankedBoardService.UpdateRankedBoardPlayerFromAudit(change, rankedPlayer);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Task.Delay(25).Wait();
                }
          

            }
        });



    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
