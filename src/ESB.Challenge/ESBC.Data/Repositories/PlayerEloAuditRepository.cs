using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using MongoDB.Driver;

namespace ESBC.Data.Repositories;

internal class PlayerEloAuditRepository : IPlayerEloAuditRepository
{
    private readonly IMongoCollection<RankedBoardPlayerRatingAudit> _playerEloAudits;

    public PlayerEloAuditRepository(IMongoCollection<RankedBoardPlayerRatingAudit> playerEloAudits)
        => _playerEloAudits = playerEloAudits;

    public async Task<RankedBoardPlayerRatingAudit?> GetByIdAsync(RankedBoardPlayerRatingAuditId id, CancellationToken ct)
    {
        var filter = Builders<RankedBoardPlayerRatingAudit>.Filter.Eq(x => x.Id, id);
        return await _playerEloAudits.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task InsertAsync(RankedBoardPlayerRatingAudit doc, CancellationToken ct)
    {
        await _playerEloAudits.InsertOneAsync(doc, cancellationToken: ct);
    }

}
