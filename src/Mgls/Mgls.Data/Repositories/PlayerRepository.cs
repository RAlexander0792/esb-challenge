using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using MongoDB.Driver;

namespace Mgls.Data.Repositories;

internal class PlayerRepository : IPlayerRepository
{
    private readonly IMongoCollection<Player> _players;

    public PlayerRepository(IMongoCollection<Player> players)
        => _players = players;

    public async Task InsertAsync(Player doc, CancellationToken ct)
    {
        await _players.InsertOneAsync(doc, cancellationToken: ct);
    }

    public async Task<Player?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<Player>.Filter.Eq(x => x.Id, id);
        return await _players.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<List<Player>> GetPlayers(int quantity, CancellationToken ct)
    {
        return await _players.Find(_ => true).Limit(quantity).ToListAsync(ct);
    }

    public async Task<List<Player>> GetByIds(IEnumerable<Guid> playerIds)
    {
        var filter = Builders<Player>.Filter.In(x => x.Id, playerIds);
        return await _players.Find(filter).ToListAsync();
    }
}
