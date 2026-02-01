using ESBC.Domain.Entities;
using Plotly.NET.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBC.Benchmarks.Charters;

public static class RatingCharter
{
    /// <summary>
    /// Generates an HTML chart showing Elo progression over time per player.
    /// Each player is rendered as a separate line.
    /// </summary>
    public static void SaveEloLinesHtml(
        IEnumerable<RankedBoardPlayerRatingAudit> audits,
        string path,
        IEnumerable<Match> matches)
    {
        // Build fast lookup table instead of First() in loop
        var matchLookup = matches.ToDictionary(m => m.Id);

        var charts = audits
            .GroupBy(a => a.Id.PlayerId)
            .OrderBy(g => g.Key.ToString())
            .Select(g =>
            {
                var xs = g.Select(a =>
                    matchLookup[a.Id.MatchId]
                        .EndedAt
                        .ToUnixTimeSeconds() / 60.0
                );

                var ys = g.Select(a => (double)a.EloAfter);

                return Chart.Line<double, double, string>(
                    x: xs,
                    y: ys,
                    Name: g.Key.ToString()[..8]
                );
            });

        var combined =
            Chart.Combine(charts)
                .WithSize(1920, 1080);

        combined.SaveHtml(path);
    }

    /// <summary>
    /// Generates a histogram of final Elo distribution across players.
    /// </summary>
    public static void SaveEloDistributionHtml(
        IEnumerable<RankedBoardPlayer> players,
        string path)
    {
        var finalElos = players
            .Select(p => (double)p.ELO)
            .ToArray();

        var histogram =
            Chart.Histogram<double, double, string>(
                X: finalElos,
                Name: "Final Elo"
            )
            .WithSize(1600, 900);

        histogram.SaveHtml(path);
    }
}
