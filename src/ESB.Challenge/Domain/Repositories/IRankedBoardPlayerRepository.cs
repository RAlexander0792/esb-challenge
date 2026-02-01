using ESBC.Domain.Entities;

namespace ESBC.Domain.Repositories;

public interface IRankedBoardPlayerRepository
{
    Task<RankedBoardPlayer?> GetByRankedBoardIdAsync(Guid id, Guid playerId, CancellationToken ct);
    Task<List<RankedBoardPlayer>> GetTopPlayers(Guid rankedBoardId, int limit);
    Task UpdateRankedBoardPlayer(RankedBoardPlayer doc);
    Task InsertAsync(RankedBoardPlayer doc, CancellationToken ct);
}
