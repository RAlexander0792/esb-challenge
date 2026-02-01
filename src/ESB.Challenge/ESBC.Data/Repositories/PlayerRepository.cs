using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using MongoDB.Driver;

namespace ESBC.Data.Repositories;

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

    public async Task<List<Player>> GetPlayers(int quantity)
    {
        return await _players.Find(_ => true).Limit(quantity).ToListAsync();
    }

}
