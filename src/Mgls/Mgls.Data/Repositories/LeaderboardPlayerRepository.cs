using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using MongoDB.Driver;

namespace Mgls.Data.Repositories;

internal class LeaderboardPlayerRepository : ILeaderboardPlayerRepository
{
    private readonly IMongoCollection<LeaderboardPlayer> _leaderboardPlayers;

    public LeaderboardPlayerRepository(IMongoCollection<LeaderboardPlayer> leaderboardPlayers)
        => _leaderboardPlayers = leaderboardPlayers;

    public async Task<LeaderboardPlayer?> GetByLeaderboardIdAsync(Guid id, Guid playerId, CancellationToken ct)
    {
        var filter = Builders<LeaderboardPlayer>.Filter.Eq(x => x.Id, new LeaderboardPlayerId(id, playerId));
        return await _leaderboardPlayers.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async  Task<List<LeaderboardPlayer>> GetByPlayerId(Guid playerId, CancellationToken ct)
    {
        var filter = Builders<LeaderboardPlayer>.Filter.Where(x => x.Id.PlayerId == playerId);
        return await _leaderboardPlayers.Find(filter).ToListAsync(ct);
    }

    public async Task<List<LeaderboardPlayer>> GetByPlayerIds(Guid leaderboardId, IEnumerable<Guid> pagePlayersIds)
    {
        var filter = Builders<LeaderboardPlayer>.Filter.Where(x => x.Id.LeaderboardId == leaderboardId && pagePlayersIds.Contains(x.Id.PlayerId));
        return await _leaderboardPlayers.Find(filter).ToListAsync();
    }

    public async Task<List<LeaderboardPlayer>> GetTopPlayers(Guid leaderboardId, int limit, CancellationToken ct)
    {
        var filter = Builders<LeaderboardPlayer>.Filter.Eq(x => x.Id.LeaderboardId, leaderboardId);
        return await _leaderboardPlayers.Find(filter).SortByDescending(x => x.Rating).Limit(limit).ToListAsync(ct);
    }

    public async Task InsertAsync(LeaderboardPlayer doc, CancellationToken ct)
    {
        await _leaderboardPlayers.InsertOneAsync(doc, cancellationToken: ct);
    }

    public async Task UpdateLeaderboardPlayer(LeaderboardPlayer doc, CancellationToken ct)
    {
        var filter = Builders<LeaderboardPlayer>.Filter
            .Eq(x => x.Id, doc.Id);

        var update = Builders<LeaderboardPlayer>.Update
            .Set(x => x.Rating, doc.Rating)
            .Set(x => x.LastAppliedMatchId, doc.LastAppliedMatchId)
            .Set(x => x.Version, doc.Version)
            .Set(x => x.Wins, doc.Wins)
            .Set(x => x.Streak, doc.Streak)
            .Set(x => x.Losses, doc.Losses)
            .Set(x => x.UpdatedAt, DateTimeOffset.UtcNow);

        await _leaderboardPlayers.UpdateOneAsync(filter, update, cancellationToken: ct);
    }
}
