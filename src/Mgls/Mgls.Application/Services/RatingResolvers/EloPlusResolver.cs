using Mgls.Domain.Entities;

namespace Mgls.Application.Services.RatingResolvers;

/// <summary>
/// A spin on the classic Elo rating calculation, with score and multiple participants support.
/// 
/// Same Expectation calculation, but a score-based Actual calculation, in which the difference in scores,
/// scaled by the constant Tau, helps determines the decisiveness of the match.
/// To support multiple players, each of them is measured against the others.
/// The K constant rating Expectation modifier is also scaled based in the amount of players,
/// to support different sizes of "Free for all" style games
/// </summary>
public class EloPlusResolver : IRatingResolver
{
    private readonly RatingRuleset _ruleset;

    public EloPlusResolver(RatingRuleset ruleset)
    {
        _ruleset = ruleset;
    }
    public List<LeaderboardPlayerRatingAudit> CalculateRatingChangesByMatch(Match match, IList<LeaderboardPlayer> participants)
    {
        var response = new List<LeaderboardPlayerRatingAudit>();
        var effectiveK = CalculateEffectiveK(_ruleset, participants.Count);

        foreach (var participant in participants)
        {
            var newPlayerEloAudit = new LeaderboardPlayerRatingAudit();
            newPlayerEloAudit.Id = new LeaderboardPlayerRatingAuditId(match.LeaderboardId, match.Id, participant.Id.PlayerId);
            newPlayerEloAudit.RulesetSnapshot = new RatingRuleset() { K = _ruleset.K, Tau = _ruleset.Tau };
            newPlayerEloAudit.RatingBefore = participant.Rating;


            CalculateParticipantExpectedResult(participant, participants.Where(x => x.Id.PlayerId != participant.Id.PlayerId), match, _ruleset, newPlayerEloAudit, effectiveK);

            var newRating = participant.Rating + effectiveK * newPlayerEloAudit.DeltaTotal;

            newPlayerEloAudit.RatingAfter = (int)Math.Round(newRating, MidpointRounding.AwayFromZero);

            response.Add(newPlayerEloAudit);

        }
        return response;
    }

    private void CalculateParticipantExpectedResult(LeaderboardPlayer participant, IEnumerable<LeaderboardPlayer> enumerable, Match match, RatingRuleset ruleset, LeaderboardPlayerRatingAudit playerAudit, double effectiveK)
    {
        // For each participant, we compare it's score against all other participans.
        var totalizerActual = 0.0;
        var totalizerExpected = 0.0;
        playerAudit.Versus = new List<LeaderboardPlayerRatingVersusAudit>();

        // This way of calculating the pairs could be more efficient, if we calculated using combinatorics
        // only the set of unique pairs. The current approach wastes calculations scaling with players per match count
        foreach (var opponent in enumerable)
        {
            var newVersus = new LeaderboardPlayerRatingVersusAudit()
            {
                PlayerId = opponent.Id.PlayerId,
                RatingBefore = opponent.Rating,

            };
            newVersus.Actual = CalculatePairActualResult(participant, opponent, match, ruleset.Tau, newVersus);
            totalizerActual += newVersus.Actual;
            newVersus.Expected = CalculatePairExpectedResult(participant, opponent);
            totalizerExpected += newVersus.Expected;
            newVersus.Delta = newVersus.Actual - newVersus.Expected;
            newVersus.RatingChangeContribution = effectiveK * newVersus.Delta;
            playerAudit.Versus.Add(newVersus);
        }
        playerAudit.ActualTotal = totalizerActual;
        playerAudit.ExpectedTotal = totalizerExpected;
        playerAudit.DeltaTotal = totalizerActual - totalizerExpected;

    }

    private double CalculatePairExpectedResult(LeaderboardPlayer a, LeaderboardPlayer b)
    {
        // Using the Elo formula for rating difference outcome expectation
        return 1.0 / (1.0 + double.Pow(10.0, (b.Rating - a.Rating) / 400.0));
    }


    private double CalculatePairActualResult(LeaderboardPlayer a,
                                             LeaderboardPlayer b,
                                             Match match,
                                             double tau,
                                             LeaderboardPlayerRatingVersusAudit versusAudit)
    {
        // Generating a value from 0-1 to represent the actual outcome difference
        var playerScore = match.PlayerScores.First(x => x.PlayerId == a.Id.PlayerId).Score;
        versusAudit.Score = match.PlayerScores.First(x => x.PlayerId == b.Id.PlayerId).Score;
        var scoreDiff = playerScore - versusAudit.Score;
        // Here we use the rating ruleset variable "Tau" to adjust how the score difference affects the decisiveness of the result
        // Higher Tau, difference in score matters less
        // Lower Tau, differences in score matters more
        return 1.0 / (1.0 + double.Pow(double.E, -scoreDiff / tau) );
    }

    private double CalculateEffectiveK(RatingRuleset ruleset, int count)
    {
        // We adjust the rating based on players, not needed for all rating calculations, but keeps
        // our system consistent when testing different player count per match
        return ruleset.K / (count - 1.0);
    }
}
