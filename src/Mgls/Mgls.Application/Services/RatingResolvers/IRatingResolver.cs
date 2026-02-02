using Mgls.Domain.Entities;

namespace Mgls.Application.Services.RatingResolvers;

/// <summary>
/// A rating resolver takes care of using a match outcome to calculate rating changes of the participants, considering their current rating
/// </summary>
public interface IRatingResolver
{
    List<LeaderboardPlayerRatingAudit> CalculateRatingChangesByMatch(Match match, IList<LeaderboardPlayer> participants);
}
