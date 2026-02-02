using Mgls.Domain.Entities;

namespace Mgls.Application.Services.RatingResolvers;

/// <summary>
/// Classic Elo rating calculation implementation. Supports only 2 player matches and no score support.
/// </summary>
public class ClassicEloResolver : IRatingResolver
{
    private readonly RatingRuleset _ruleset;

    public ClassicEloResolver(RatingRuleset ruleset)
    {
        _ruleset = ruleset;
    }
    public List<LeaderboardPlayerRatingAudit> CalculateRatingChangesByMatch(Match match, IList<LeaderboardPlayer> participants)
    {
        var response = new List<LeaderboardPlayerRatingAudit>();

        if (participants.Count != 2)
        {
            throw new ArgumentException("Player count not supported");
        }

        // Player A
        var playerAAudit = new LeaderboardPlayerRatingAudit();
        playerAAudit.Id = new LeaderboardPlayerRatingAuditId(match.LeaderboardId, match.Id, participants[0].Id.PlayerId);
        playerAAudit.RulesetSnapshot = new RatingRuleset() { K = _ruleset.K, Tau = _ruleset.Tau };
        playerAAudit.RatingBefore = participants[0].Rating;
        playerAAudit.Versus = new List<LeaderboardPlayerRatingVersusAudit>();
        playerAAudit.Score = match.PlayerScores.First(x => x.PlayerId == participants[0].Id.PlayerId).Score;
        var newVersus = new LeaderboardPlayerRatingVersusAudit()
        {
            PlayerId = participants[1].Id.PlayerId,
            RatingBefore = participants[1].Rating,
        };
        
        // For Both
        var expectedResult = CalculatePairExpectedResult(participants[0], participants[1]);
        var actualResult = CalculatePairActualResult(participants[0], participants[1], match, newVersus);

        // Player A
        newVersus.Delta = newVersus.Actual - newVersus.Expected;
        newVersus.RatingChangeContribution = _ruleset.K * newVersus.Delta;
        playerAAudit.ExpectedTotal = expectedResult;
        playerAAudit.ActualTotal = actualResult;
        playerAAudit.DeltaTotal = playerAAudit.ExpectedTotal - playerAAudit.ActualTotal;
        var newRating = participants[0].Rating + _ruleset.K * playerAAudit.DeltaTotal;
        playerAAudit.RatingAfter = (int)Math.Round(newRating, MidpointRounding.AwayFromZero);
        response.Add(playerAAudit);

        // Player B
        var playerBAudit = new LeaderboardPlayerRatingAudit();
        playerBAudit.Id = new LeaderboardPlayerRatingAuditId(match.LeaderboardId, match.Id, participants[1].Id.PlayerId);
        playerBAudit.RulesetSnapshot = new RatingRuleset() { K = _ruleset.K, Tau = _ruleset.Tau };
        playerBAudit.RatingBefore = participants[1].Rating;
        playerBAudit.Versus = new List<LeaderboardPlayerRatingVersusAudit>();
        playerBAudit.Score = match.PlayerScores.First(x => x.PlayerId == participants[1].Id.PlayerId).Score;
        
        var newVersusB = new LeaderboardPlayerRatingVersusAudit()
        {
            PlayerId = participants[0].Id.PlayerId,
            RatingBefore = participants[0].Rating,
        };
        
        playerBAudit.ExpectedTotal = 1 - expectedResult;
        playerBAudit.ActualTotal = 1 - actualResult;
        playerBAudit.DeltaTotal = playerBAudit.ExpectedTotal - playerBAudit.ActualTotal;
        newRating = participants[1].Rating + _ruleset.K * playerBAudit.DeltaTotal;

        playerBAudit.RatingAfter = (int)Math.Round(newRating, MidpointRounding.AwayFromZero);
        newVersusB.Delta = newVersusB.Actual - newVersusB.Expected;
        newVersusB.RatingChangeContribution = _ruleset.K * newVersusB.Delta;
        playerBAudit.Versus.Add(newVersusB);

        response.Add(playerBAudit);

        return response;
    }

    private double CalculatePairExpectedResult(LeaderboardPlayer a, LeaderboardPlayer b)
    {
        // Using the Elo formula for rating difference outcome expectation
        return 1.0 / (1.0 + double.Pow(10.0, (b.Rating - a.Rating) / 400.0));
    }


    private double CalculatePairActualResult(LeaderboardPlayer a,
                                             LeaderboardPlayer b,
                                             Match match,
                                             LeaderboardPlayerRatingVersusAudit versusAudit)
    {
        // As we don't care of the score, we use a same number on both scores as a draw or greater number as a winner
        var playerScore = match.PlayerScores.First(x => x.PlayerId == a.Id.PlayerId).Score;
        versusAudit.Score = match.PlayerScores.First(x => x.PlayerId == b.Id.PlayerId).Score;
        if (playerScore > versusAudit.Score)
            return 1.0;
        else if (playerScore == versusAudit.Score)
            return 0.5;
        else
            return 0.0;
    }
}
