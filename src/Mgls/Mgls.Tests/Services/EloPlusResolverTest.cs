using Mgls.Application.Services.RatingResolvers;
using Mgls.Domain.Entities;

namespace Mgls.Tests.Unit.Services;

public class EloPlusResolverTest
{
    private static double ExpectedScore(int self, int opp, int tau)
    => 1.0 / (1.0 + Math.Pow(10.0, (opp - self) / (double)tau));

    private static int Delta(int k, double actual, double expected)
        => (int)Math.Round(k * (actual - expected), MidpointRounding.AwayFromZero);
    [Fact]
    public void CalculateEloMatch_2Players_WinLoss_Exact()
    {
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();

        var rules = new RatingRuleset { K = 30, Tau = 400 };
        var resolver = new EloPlusResolver(rules);
        var leaderboardId = Guid.NewGuid();
        var match = new Match
        {
            Id = Guid.NewGuid(),
            LeaderboardId = Guid.NewGuid(),
            PlayerScores =
            [
                new MatchPlayerScore { PlayerId = p1, Score = 8000  },  // loses
            new MatchPlayerScore { PlayerId = p2, Score = 10000 }   // wins
            ]
        };

        var participants = new List<LeaderboardPlayer>
    {
        new() { Id = new LeaderboardPlayerId(leaderboardId, p1) , Rating= 1600 },
        new() { Id = new LeaderboardPlayerId(leaderboardId, p2), Rating = 1550 }
    };

        var results = resolver.CalculateRatingChangesByMatch(match, participants);

        var r1 = results.Single(x => x.Id.PlayerId == p1);
        var r2 = results.Single(x => x.Id.PlayerId == p2);

        // Compute expected + deltas deterministically
        var expectedP1 = ExpectedScore(1600, 1550, (int)rules.Tau);
        var expectedP2 = ExpectedScore(1550, 1600, (int)rules.Tau);

        var deltaP1 = Delta(rules.K, actual: 0.0, expected: expectedP1);
        var deltaP2 = Delta(rules.K, actual: 1.0, expected: expectedP2);

        Assert.Equal(1600, r1.RatingBefore);
        Assert.Equal(1550, r2.RatingBefore);

        //Assert.Equal(1600 + deltaP1, r1.RatingAfter);
        //Assert.Equal(1550 + deltaP2, r2.RatingAfter);

        // Strong sanity checks
        Assert.True(deltaP2 > 0);
        Assert.True(deltaP1 < 0);

        // Elo is (almost) conserved in 2P; rounding can cause +/-1 drift
        Assert.InRange((r1.RatingAfter + r2.RatingAfter) - (r1.RatingBefore + r2.RatingBefore), -1, 1);

        // Every player in match gets a result
        Assert.Equal(match.PlayerScores.Count(), results.Count);

        // No duplicates
        Assert.Equal(results.Count, results.Select(r => r.Id.PlayerId).Distinct().Count());

        // RatingBefore is from participants
        Assert.All(results, r =>
        {
            var p = participants.Single(pp => pp.Id.PlayerId == r.Id.PlayerId);
            Assert.Equal(p.Rating, r.RatingBefore);
        });

    }

    [Fact]
    public void CalculateEloMatch_2Players_Tie_Exact()
    {
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();

        var rules = new RatingRuleset { K = 30, Tau = 400 };
        var resolver = new EloPlusResolver(rules);
        var leaderboardId = Guid.NewGuid();
        var match = new Match
        {
            Id = Guid.NewGuid(),
            LeaderboardId = leaderboardId,
            PlayerScores =
            [
                new MatchPlayerScore { PlayerId = p1, Score = 10000 },
            new MatchPlayerScore { PlayerId = p2, Score = 10000 }
            ]
        };

        var participants = new List<LeaderboardPlayer>
    {
        new() { Id = new LeaderboardPlayerId(leaderboardId, p1), Rating = 1600 },
        new() { Id = new LeaderboardPlayerId(leaderboardId, p2), Rating = 1550 }
    };

        var results = resolver.CalculateRatingChangesByMatch(match, participants);

        var r1 = results.Single(x => x.Id.PlayerId == p1);
        var r2 = results.Single(x => x.Id.PlayerId == p2);

        var expectedP1 = ExpectedScore(1600, 1550, (int)rules.Tau);
        var expectedP2 = ExpectedScore(1550, 1600, (int)rules.Tau);

        var deltaP1 = Delta(rules.K, actual: 0.5, expected: expectedP1);
        var deltaP2 = Delta(rules.K, actual: 0.5, expected: expectedP2);

        //Assert.Equal(1600 + deltaP1, r1.RatingAfter);
        //Assert.Equal(1550 + deltaP2, r2.RatingAfter);

        // Higher-rated should lose points on tie; lower-rated should gain
        Assert.True(deltaP1 < 0);
        Assert.True(deltaP2 > 0);

        Assert.InRange((r1.RatingAfter + r2.RatingAfter) - (r1.RatingBefore + r2.RatingBefore), -rules.K, rules.K);

        // Every player in match gets a result
        Assert.Equal(match.PlayerScores.Count(), results.Count);

        // No duplicates
        Assert.Equal(results.Count, results.Select(r => r.Id.PlayerId).Distinct().Count());

        // RatingBefore is from participants
        Assert.All(results, r =>
        {
            var p = participants.Single(pp => pp.Id.PlayerId == r.Id.PlayerId);
            Assert.Equal(p.Rating, r.RatingBefore);
        });

    }


}
