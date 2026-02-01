using ESBC.Application.Services.RatingResolvers;
using ESBC.Domain.Entities;

namespace ESBC.Application.Factories;

public interface IEloResolverFactory
{
    IRatingResolver CreateClassicResolver(EloRulesetSnapshot ruleset);
}
