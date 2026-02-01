using ESBC.Domain.Entities;

namespace ESBC.Application.Services.RatingResolvers;

public class ClassicEloResolver : IRatingResolver
{
    private readonly EloRulesetSnapshot ruleset;

    public ClassicEloResolver(EloRulesetSnapshot ruleset)
    {
        this.ruleset = ruleset;
    }
    public List<RankedBoardPlayerRatingAudit> CalculateRatingChangesByMatch(Match match, IList<RankedBoardPlayer> participants)
    {
        var response = new List<RankedBoardPlayerRatingAudit>();
        var effectiveK = CalculateEffectiveK(ruleset, participants.Count);

        foreach (var participant in participants)
        {
            var newPlayerEloAudit = new RankedBoardPlayerRatingAudit();
            newPlayerEloAudit.Id = new RankedBoardPlayerRatingAuditId(match.RankedBoardId, match.Id, participant.Id.PlayerId);
            newPlayerEloAudit.RulesetSnapshot = new EloRulesetSnapshot() { K = ruleset.K, Tau = ruleset.Tau };
            newPlayerEloAudit.EloBefore = participant.ELO;


            CalculateParticipantExpectedResult(participant, participants.Where(x => x.Id.PlayerId != participant.Id.PlayerId), match, ruleset, newPlayerEloAudit, effectiveK);

            var newRating = participant.ELO + effectiveK * newPlayerEloAudit.DeltaTotal;

            newPlayerEloAudit.EloAfter = (int)Math.Round(newRating, MidpointRounding.AwayFromZero);

            response.Add(newPlayerEloAudit);

        }
        return response;
    }

    private void CalculateParticipantExpectedResult(RankedBoardPlayer participant, IEnumerable<RankedBoardPlayer> enumerable, Match match, EloRulesetSnapshot ruleset, RankedBoardPlayerRatingAudit playerAudit, double effectiveK)
    {

        var totalizerActual = 0.0;
        var totalizerExpected = 0.0;
        playerAudit.Versus = new List<PlayerEloAuditVersus>();
        foreach (var opponent in enumerable)
        {
            var newVersus = new PlayerEloAuditVersus()
            {
                OpponentPlayerId = opponent.Id.PlayerId,
                OpponentEloBefore = opponent.ELO,

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

    private double CalculatePairExpectedResult(RankedBoardPlayer a, RankedBoardPlayer b)
    {
        return 1 / (1 + MathF.Pow(10, b.ELO - a.ELO) / 400);
    }


    private double CalculatePairActualResult(RankedBoardPlayer a, RankedBoardPlayer b, Match match, double tau, PlayerEloAuditVersus versusAudit)
    {
        var playerScore = match.PlayerScores.First(x => x.PlayerId == a.Id.PlayerId).Score;
        versusAudit.OpponentScore = match.PlayerScores.First(x => x.PlayerId == b.Id.PlayerId).Score;
        versusAudit.ScoreDiff = playerScore - versusAudit.OpponentScore;
        return 1 / (1 + MathF.Pow(MathF.E, -versusAudit.ScoreDiff) / tau);
    }

    private double CalculateEffectiveK(EloRulesetSnapshot ruleset, int count)
    {
        return ruleset.K / (count - 1);
    }
}
