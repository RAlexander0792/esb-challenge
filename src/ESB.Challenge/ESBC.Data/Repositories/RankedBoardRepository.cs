using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using MongoDB.Driver;

namespace ESBC.Data.Repositories;

internal class RankedBoardRepository : IRankedBoardRepository
{
    private readonly IMongoCollection<RankedBoard> _rankedBoards;

    public RankedBoardRepository(IMongoCollection<RankedBoard> rankedBoards)
        => _rankedBoards = rankedBoards;

    public async Task<RankedBoard?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<RankedBoard>.Filter.Eq(x => x.Id, id);
        return await _rankedBoards.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<List<RankedBoard>> GetRankedBoards()
    {
        return await _rankedBoards.Find(_ => true).ToListAsync();
    }

    public async Task InsertAsync(RankedBoard doc, CancellationToken ct)
    {
        await _rankedBoards.InsertOneAsync(doc, cancellationToken: ct);
    }

}
