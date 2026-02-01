using ESBC.Domain.Entities;

namespace ESBC.Application.Services.RatingResolvers;

public interface IRatingResolver
{
    List<RankedBoardPlayerRatingAudit> CalculateRatingChangesByMatch(Match match, IList<RankedBoardPlayer> participants);
}
