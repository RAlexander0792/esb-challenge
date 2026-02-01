using ESBC.Application.Services.RatingResolvers;
using ESBC.Domain.Entities;

namespace ESBC.Application.Factories;

public class EloResolverFactory : IEloResolverFactory
{
    public IRatingResolver CreateClassicResolver(EloRulesetSnapshot ruleset)
    {
        var resolver = new ClassicEloResolver(ruleset);


        return resolver;
    }
}
