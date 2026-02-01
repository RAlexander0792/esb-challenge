using ESBC.Domain.Entities;
using ESBC.Domain.Services.Dtos;

namespace ESBC.Domain.Services;

public interface IRankedBoardService
{
    Task<RankedBoard> CreateRankedBoard(CreateRankedBoardDto rankedBoard);
    Task<List<RankedBoardPlayer>> GetRankedBoardPlayers(Guid rankedBoardId, IEnumerable<Guid> playerIds, CancellationToken ct);
    Task<List<RankedBoard>> GetRankedBoards();
    Task<List<RankedBoardPlayer>> GetTopPlayers(Guid rankedBoardId, int limit);
    Task<RankedBoardPlayer> InitializeRankedPlayer(Guid rankedBoardId, Guid nonRankedPlayer);
    Task UpdateRankedBoardPlayerFromAudit(RankedBoardPlayerRatingAudit record, RankedBoardPlayer rankedPlayer);
}
