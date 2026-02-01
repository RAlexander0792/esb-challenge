using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using MongoDB.Driver;

namespace ESBC.Data.Repositories;

internal class RankedBoardPlayerRepository : IRankedBoardPlayerRepository
{
    private readonly IMongoCollection<RankedBoardPlayer> _rankedBoardPlayers;

    public RankedBoardPlayerRepository(IMongoCollection<RankedBoardPlayer> rankedBoardPlayers)
        => _rankedBoardPlayers = rankedBoardPlayers;

    public async Task<RankedBoardPlayer?> GetByRankedBoardIdAsync(Guid id, Guid playerId, CancellationToken ct)
    {
        var filter = Builders<RankedBoardPlayer>.Filter.Eq(x => x.Id, new RankedBoardPlayerId(id, playerId));
        return await _rankedBoardPlayers.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<List<RankedBoardPlayer>> GetTopPlayers(Guid rankedBoardId, int limit)
    {
        var filter = Builders<RankedBoardPlayer>.Filter.Eq(x => x.Id.RankedBoardId, rankedBoardId);
        return await _rankedBoardPlayers.Find(filter).SortByDescending(x => x.ELO).Limit(limit).ToListAsync();
    }

    public async Task InsertAsync(RankedBoardPlayer doc, CancellationToken ct)
    {
        await _rankedBoardPlayers.InsertOneAsync(doc, cancellationToken: ct);
    }

    public async Task UpdateRankedBoardPlayer(RankedBoardPlayer doc)
    {
        var filter = Builders<RankedBoardPlayer>.Filter
            .Eq(x => x.Id, doc.Id);

        var update = Builders<RankedBoardPlayer>.Update
            .Set(x => x.ELO, doc.ELO)
            .Set(x => x.LastAppliedMatchId, doc.LastAppliedMatchId)
            .Set(x => x.Version, doc.Version)
            .Set(x => x.Wins, doc.Wins)
            .Set(x => x.Losses, doc.Losses)
            .Set(x => x.UpdatedAt, DateTimeOffset.UtcNow);

        await _rankedBoardPlayers.UpdateOneAsync(filter, update, cancellationToken: CancellationToken.None);
    }
}
