using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using MongoDB.Driver;

namespace Mgls.Data.Repositories;

internal class LeaderboardRepository : ILeaderboardRepository
{
    private readonly IMongoCollection<Leaderboard> _leaderboards;

    public LeaderboardRepository(IMongoCollection<Leaderboard> leaderboards)
        => _leaderboards = leaderboards;

    public async Task<Leaderboard?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<Leaderboard>.Filter.Eq(x => x.Id, id);
        return await _leaderboards.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<List<Leaderboard>> GetLeaderboards(CancellationToken ct)
    {
        return await _leaderboards.Find(_ => true).ToListAsync(ct);
    }

    public async Task InsertAsync(Leaderboard doc, CancellationToken ct)
    {
        await _leaderboards.InsertOneAsync(doc, cancellationToken: ct);
    }

}
