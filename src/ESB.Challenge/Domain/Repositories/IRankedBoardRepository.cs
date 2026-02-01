using ESBC.Domain.Entities;

namespace ESBC.Domain.Repositories;

public interface IRankedBoardRepository
{
    Task<RankedBoard?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<RankedBoard>> GetRankedBoards();
    Task InsertAsync(RankedBoard doc, CancellationToken ct);
}
