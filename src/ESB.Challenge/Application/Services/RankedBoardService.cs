using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;

namespace ESBC.Application.Services;

public class RankedBoardService : IRankedBoardService
{
    private readonly IRankedBoardRepository _rankedBoardRepository;
    private readonly IRankedBoardPlayerRepository _rankedBoardPlayerRepository;
    private readonly IPlayerService _playerService;

    public RankedBoardService(IRankedBoardRepository rankedBoardRepository,
                              IRankedBoardPlayerRepository rankedBoardPlayerRepository,
                              IPlayerService playerService)
    {
        _rankedBoardRepository = rankedBoardRepository;
        _rankedBoardPlayerRepository = rankedBoardPlayerRepository;
        _playerService = playerService;
    }

    public async Task<RankedBoard> CreateRankedBoard(CreateRankedBoardDto rankedBoard)
    {
        var newRankedBoard = new RankedBoard()
        {
            Name = rankedBoard.Name,
            Description = "Test"
        };

        await _rankedBoardRepository.InsertAsync(newRankedBoard, new CancellationToken());

        return newRankedBoard;
    }

    public async Task<List<RankedBoardPlayer>> GetRankedBoardPlayers(Guid rankedBoardId, IEnumerable<Guid> playerIds, CancellationToken ct)
    {
        var players = new List<RankedBoardPlayer>();

        foreach (var playerId in playerIds)
        {
            var playerDb = await _rankedBoardPlayerRepository.GetByRankedBoardIdAsync(rankedBoardId, playerId, ct);
            players.Add(playerDb);
        }

        return players;
    }

    public async Task<List<RankedBoard>> GetRankedBoards()
    {
        return await _rankedBoardRepository.GetRankedBoards();
    }

    public Task<List<RankedBoardPlayer>> GetTopPlayers(Guid rankedBoardId, int limit)
    {
        return _rankedBoardPlayerRepository.GetTopPlayers(rankedBoardId, limit);
    }

    public async Task<RankedBoardPlayer> InitializeRankedPlayer(Guid rankedBoardId, Guid nonRankedPlayer)
    {
        if (await _playerService.DoesPlayerExists(nonRankedPlayer)
           && await RankedBoardExists(rankedBoardId))
        {
            RankedBoardPlayer? newRankedPlayer = new RankedBoardPlayer()
            {
                ELO = 1500,
                Losses = 0,
                Wins = 0,
                LastAppliedMatchId = Guid.Empty,
                Id = new RankedBoardPlayerId(rankedBoardId, nonRankedPlayer),
                UpdatedAt = DateTime.UtcNow,
                Version = 0
            };
            
            await _rankedBoardPlayerRepository.InsertAsync(newRankedPlayer, CancellationToken.None);
            return newRankedPlayer;
        }
        return null;
    }

    public async Task UpdateRankedBoardPlayerFromAudit(RankedBoardPlayerRatingAudit record, RankedBoardPlayer rankedPlayer)
    {
        rankedPlayer.ELO = record.EloAfter;
        rankedPlayer.LastAppliedMatchId = record.Id.MatchId;
        rankedPlayer.Version++;

        if (record.DeltaTotal > 0)
            rankedPlayer.Wins++;
        else    
            rankedPlayer.Losses++;

        await _rankedBoardPlayerRepository.UpdateRankedBoardPlayer(rankedPlayer);
    }

    private async Task<bool> RankedBoardExists(Guid rankedBoardId)
    {
        return await _rankedBoardRepository.GetByIdAsync(rankedBoardId, CancellationToken.None) != null;
    }
}
