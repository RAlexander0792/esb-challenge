using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using MongoDB.Driver;

namespace ESBC.Data.Repositories;

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

    public async Task InsertAsync(Match doc, CancellationToken ct)
    {
        await _matches.InsertOneAsync(doc, cancellationToken: ct);
    }

}
