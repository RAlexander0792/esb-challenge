using Mgls.Application.Services.RatingResolvers;
using Mgls.Domain.Entities;

namespace Mgls.Application.Factories;

public class RatingResolverFactory : IRatingResolverFactory
{
    public IRatingResolver CreateClassicEloResolver(RatingRuleset ruleset)
    {
        var resolver = new ClassicEloResolver(ruleset);
        return resolver;
    }

    public IRatingResolver CreateEloPlusResolver(RatingRuleset ruleset)
    {
        var resolver = new EloPlusResolver(ruleset);
        return resolver;
    }
}
