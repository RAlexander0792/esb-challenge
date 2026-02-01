using BenchmarkDotNet.Attributes;
using ESBC.Application.Services.RatingResolvers;
using ESBC.Benchmarks.Charters;
using ESBC.Benchmarks.EntityBuilders;
using ESBC.Domain.Entities;

namespace ESBC.Benchmarks.Resolvers;

public class ClassicEloResolverBench
{
    public bool Plot = false;
    [Benchmark]
    public void CalculateRating_2Players_100Matches_25PlayerPool()
    {
        EloRulesetSnapshot ruleset = new EloRulesetSnapshot();

        ruleset.Tau = 400;
        ruleset.K = 15;

        var resolver = new ClassicEloResolver(ruleset);

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
                player.ELO = e.EloAfter;
            }

        }

    }

    public void PlotRating_2Players_100Matches_25PlayerPool()
    {
        EloRulesetSnapshot ruleset = new EloRulesetSnapshot();

        ruleset.Tau = 400;
        ruleset.K = 30;

        var resolver = new ClassicEloResolver(ruleset);

        var playerBuilder = new PlayerBuilder(1234);
        playerBuilder.WithRatingRange(1200, 1300);

        var playerPool = playerBuilder.BuildMany(200);

        var matchBuilder = new MatchBuilder(1234);

        matchBuilder.WithPlayersPerMatch(2);
        matchBuilder.WithPlayers(playerPool);
        matchBuilder.WithScoreRange(1000, 2000);

        var matches = matchBuilder.BuildMany(10000);
        matches = matches.OrderBy(x => x.EndedAt).ToList();

        var allChanges = new List<RankedBoardPlayerRatingAudit>();
        foreach (var match in matches)
        {
            var participants = playerPool.Where(x => match.PlayerScores.Select(c => c.PlayerId).Contains(x.Id.PlayerId)).ToList();

            var matchRatingChanges = resolver.CalculateRatingChangesByMatch(match, participants);
            foreach (var e in matchRatingChanges)
            {
                var player = playerPool.FirstOrDefault(x => x.Id.PlayerId == e.Id.PlayerId);
                player.ELO = e.EloAfter;
                allChanges.Add(e);
            }

        }
        RatingCharter.SaveEloLinesHtml(allChanges, "C:\\Users\\MakubeX\\Desktop\\Plots\\eloLines.html", matches);

        RatingCharter.SaveEloDistributionHtml(playerPool, "C:\\Users\\MakubeX\\Desktop\\Plots\\distribution.html");

    }


}
