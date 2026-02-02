using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using MongoDB.Driver;

namespace Mgls.Data.Repositories;

internal class LeaderboardPlayerRatingAuditRepository : ILeaderboardPlayerRatingAuditRepository
{
    private readonly IMongoCollection<LeaderboardPlayerRatingAudit> _playerEloAudits;

    public LeaderboardPlayerRatingAuditRepository(IMongoCollection<LeaderboardPlayerRatingAudit> playerEloAudits)
        => _playerEloAudits = playerEloAudits;

    public async Task<LeaderboardPlayerRatingAudit?> GetByIdAsync(LeaderboardPlayerRatingAuditId id, CancellationToken ct)
    {
        var filter = Builders<LeaderboardPlayerRatingAudit>.Filter.Eq(x => x.Id, id);
        return await _playerEloAudits.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task InsertAsync(LeaderboardPlayerRatingAudit doc, CancellationToken ct)
    {
        await _playerEloAudits.InsertOneAsync(doc, cancellationToken: ct);
    }

}
