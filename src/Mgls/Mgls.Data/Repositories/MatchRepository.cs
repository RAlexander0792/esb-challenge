using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using MongoDB.Driver;

namespace Mgls.Data.Repositories;

internal class MatchRepository : IMatchRepository
{
    private readonly IMongoCollection<Match> _matches;

    public MatchRepository(IMongoCollection<Match> matches)
        => _matches = matches;

    public async Task<Match?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<Match>.Filter.Eq(x => x.Id, id);
        return await _matches.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<List<Match>> GetByLeaderboard(Guid leaderboardId, int limit, CancellationToken ct)
    {
        var filter = Builders<Match>.Filter.Eq(x => x.LeaderboardId, leaderboardId);
        return await _matches.Find(filter)
            .Sort(Builders<Match>.Sort.Descending(x => x.EndedAt))
            .Limit(limit)
            .ToListAsync(ct);
    }

    public async Task<List<Match>> GetByPlayer(Guid playerId, Guid? leaderboardId, int limit, CancellationToken ct)
    {
        var filter = Builders<Match>.Filter.Where(x => x.PlayerScores.Any(c => c.PlayerId == playerId));
        if (leaderboardId != null)
            filter &= Builders<Match>.Filter.Eq(x => x.LeaderboardId, leaderboardId.Value);

        return await _matches.Find(filter)
            .Sort(Builders<Match>.Sort.Descending(x => x.EndedAt))
            .Limit(limit)
            .ToListAsync(ct);
    }

    public async Task InsertAsync(Match doc, CancellationToken ct)
    {
        await _matches.InsertOneAsync(doc, cancellationToken: ct);
    }

}
