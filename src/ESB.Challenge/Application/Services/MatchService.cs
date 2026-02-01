using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;
using System.Threading.Channels;

namespace ESBC.Application.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly Channel<Match> _channel;

    public MatchService(IMatchRepository matchRepository, Channel<Match> channel)
    {
        _matchRepository = matchRepository;
        _channel = channel;
    }

    public async Task<Match> CreateMatch(CreateMatchDto match)
    {
        var newMatch = new Match()
        {
            CreatedAt = DateTime.Now,
            RankedBoardId = match.RankedBoardId,
            PlayerScores = match.PlayerScores,
        };

        await _matchRepository.InsertAsync(newMatch, new CancellationToken());

        await _channel.Writer.WriteAsync(newMatch);

        return newMatch;
    }


}
