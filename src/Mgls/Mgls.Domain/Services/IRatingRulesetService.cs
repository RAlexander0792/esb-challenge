using Mgls.Domain.Entities;

namespace Mgls.Domain.Services;

public interface IRatingRulesetService
{
    Task<RatingRuleset> Create(RatingRuleset ratingRuleset);
    Task<RatingRuleset> GetById(Guid ratingRulesetId, CancellationToken ct);
}
