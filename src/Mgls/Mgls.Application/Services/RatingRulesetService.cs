using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using Mgls.Domain.Services;

namespace Mgls.Application.Services;

public class RatingRulesetService : IRatingRulesetService
{
    private readonly IRatingRulesetRepository _ratingRulesetRepository;

    public RatingRulesetService(IRatingRulesetRepository ratingRulesetRepository)
    {
        _ratingRulesetRepository = ratingRulesetRepository;
    }

    public async Task<RatingRuleset> Create(RatingRuleset ratingRuleset)
    {
        await _ratingRulesetRepository.InsertAsync(ratingRuleset, new CancellationToken());
        return ratingRuleset;
    }

    public async Task<RatingRuleset> GetById(Guid ratingRulesetId, CancellationToken ct)
    {
        return await _ratingRulesetRepository.GetByIdAsync(ratingRulesetId, ct);
    }

}
