using Mgls.Domain.Entities;

namespace Mgls.Domain.Repositories;

public interface IRatingRulesetRepository
{
    Task<bool> Exists(Guid ratingRulesetId);
    public Task<RatingRuleset?> GetByIdAsync(Guid id, CancellationToken ct);
    public Task InsertAsync(RatingRuleset doc, CancellationToken ct);
}
