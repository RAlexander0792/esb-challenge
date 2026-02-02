using BenchmarkDotNet.Attributes;
using Mgls.Application.Services.RatingResolvers;
using Mgls.Benchmarks.Charters;
using Mgls.Benchmarks.EntityBuilders;
using Mgls.Domain.Entities;

namespace Mgls.Benchmarks.Resolvers;

public class ClassicEloResolverBench
{
    public bool Plot = false;
    [Benchmark]
    public void CalculateRating_2Players_100Matches_25PlayerPool()
    {
        RatingRuleset ruleset = new RatingRuleset();

        ruleset.Tau = 400;
        ruleset.K = 15;

        var resolver = new EloPlusResolver(ruleset);

        var playerBuilder = new PlayerBuilder(1234);
        playerBuilder.WithRatingRange(0, 2000);
        
        var playerPool = playerBuilder.BuildMany(25);

        var matchBuilder = new MatchBuilder(1234);

        matchBuilder.WithPlayersPerMatch(2);
        matchBuilder.WithPlayers(playerPool);
        matchBuilder.WithScoreRange(600, 1800);
        
        var matches = matchBuilder.BuildMany(100);
        matches = matches.OrderBy(x => x.EndedAt).ToList();
        foreach (var match in matches)
        {
            var participants = playerPool.Where(x => match.PlayerScores.Select(c => c.PlayerId).Contains(x.Id.PlayerId)).ToList();

            var matchRatingChanges = resolver.CalculateRatingChangesByMatch(match, participants);

            foreach(var e in matchRatingChanges)
            {
                var player = playerPool.FirstOrDefault(x => x.Id.PlayerId == e.Id.PlayerId);
                player.Rating = e.RatingAfter;
            }

        }

    }

    public void PlotRating_2Players_100Matches_25PlayerPool()
    {
        RatingRuleset ruleset = new RatingRuleset();

        ruleset.Tau = 400;
        ruleset.K = 30;

        var resolver = new EloPlusResolver(ruleset);

        var playerBuilder = new PlayerBuilder(1234);
        playerBuilder.WithRatingRange(1200, 1300);

        var playerPool = playerBuilder.BuildMany(200);

        var matchBuilder = new MatchBuilder(1234);

        matchBuilder.WithPlayersPerMatch(2);
        matchBuilder.WithPlayers(playerPool);
        matchBuilder.WithScoreRange(1000, 2000);

        var matches = matchBuilder.BuildMany(10000);
        matches = matches.OrderBy(x => x.EndedAt).ToList();

        var allChanges = new List<LeaderboardPlayerRatingAudit>();
        foreach (var match in matches)
        {
            var participants = playerPool.Where(x => match.PlayerScores.Select(c => c.PlayerId).Contains(x.Id.PlayerId)).ToList();

            var matchRatingChanges = resolver.CalculateRatingChangesByMatch(match, participants);
            foreach (var e in matchRatingChanges)
            {
                var player = playerPool.FirstOrDefault(x => x.Id.PlayerId == e.Id.PlayerId);
                player.Rating = e.RatingAfter;
                allChanges.Add(e);
            }

        }
        RatingCharter.SaveEloLinesHtml(allChanges, "eloLines.html", matches);

        RatingCharter.SaveEloDistributionHtml(playerPool, "distribution.html");

    }


}
