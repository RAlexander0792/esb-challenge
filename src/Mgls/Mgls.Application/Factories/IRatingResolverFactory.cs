using Mgls.Application.Services.RatingResolvers;
using Mgls.Domain.Entities;

namespace Mgls.Application.Factories;

public interface IRatingResolverFactory
{
    IRatingResolver CreateClassicEloResolver(RatingRuleset ruleset);
    IRatingResolver CreateEloPlusResolver(RatingRuleset ruleset);
}
