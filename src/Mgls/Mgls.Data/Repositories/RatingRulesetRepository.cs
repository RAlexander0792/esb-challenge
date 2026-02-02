using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using MongoDB.Driver;

namespace Mgls.Data.Repositories;

internal class RatingRulesetRepository : IRatingRulesetRepository
{
    private readonly IMongoCollection<RatingRuleset> _ratingRulesets;

    public RatingRulesetRepository(IMongoCollection<RatingRuleset> ratingRulesets)
        => _ratingRulesets = ratingRulesets;

    public async Task<bool> Exists(Guid ratingRulesetId)
    {
        return await _ratingRulesets.Find(x => x.Id == ratingRulesetId)
                                    .Limit(1)
                                    .AnyAsync();
    }

    public async Task<RatingRuleset?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<RatingRuleset>.Filter.Eq(x => x.Id, id);
        return await _ratingRulesets.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task InsertAsync(RatingRuleset doc, CancellationToken ct)
    {
        await _ratingRulesets.InsertOneAsync(doc, cancellationToken: ct);
    }

}
