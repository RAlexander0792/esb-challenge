using Mgls.Domain.Entities;

namespace Mgls.Benchmarks.EntityBuilders;

public class MatchBuilder
{
    private readonly Random Rng;

    DateTimeOffset StartedAt_FromDate = DateTimeOffset.UtcNow.AddMinutes(-40);
    DateTimeOffset StartedAt_ToDate = DateTimeOffset.UtcNow;
    Guid _leaderboardId;
    List<LeaderboardPlayer> _players;
    long _scoreRangeFrom;
    long _scoreRangeTo;
    int _playersPerMatch;

    public MatchBuilder(int seed)
    {
        Rng = new Random(seed);
    }

    public MatchBuilder ForLeaderboard(Guid leaderboardId)
    {
        _leaderboardId = leaderboardId;
        return this;
    }

    public MatchBuilder WithPlayers(List<LeaderboardPlayer> players)
    {
        _players = players;
        return this;
    }

    public MatchBuilder WithPlayersPerMatch(int playersPerMatch)
    {
        _playersPerMatch = playersPerMatch;
        return this;
    }

    public MatchBuilder WithScoreRange(long from, long to) 
    {
        _scoreRangeFrom = from;
        _scoreRangeTo = to;
        return this;
    }

    public MatchBuilder InDateRange(DateTimeOffset from, DateTimeOffset to)
    {
        StartedAt_FromDate = from;
        StartedAt_ToDate = to;
        return this;
    }


    public Match BuildOne()
    {
        var newMatch = new Match();
        newMatch.LeaderboardId = _leaderboardId;
        newMatch.StartedAt = RandomDate(StartedAt_FromDate.DateTime, StartedAt_ToDate.DateTime);
        newMatch.CreatedAt = newMatch.StartedAt.Add(-TimeSpan.FromSeconds(Rng.Next(0, 600)));
        //newMatch.EndedAt = newMatch.StartedAt.Add(TimeSpan.FromSeconds(Rng.Next(0, 600)));
        newMatch.PlayerScores = new List<MatchPlayerScore>();
        newMatch.Id = Guid.NewGuid();

        var playersInMatch = new List<LeaderboardPlayer>();

        while(playersInMatch.Count < _playersPerMatch) 
        {
            var tentativePlayer = _players[Rng.Next(_players.Count)];
            if (!playersInMatch.Any(x => x.Id.PlayerId == tentativePlayer.Id.PlayerId))
            {
                playersInMatch.Add(tentativePlayer);
            }
        }

        for (int i = 0; i < _playersPerMatch; i++)
        {
            var newScore = GeneratePlayerScore(playersInMatch[i].Id.PlayerId);
            newMatch.PlayerScores.Add(newScore);
        }


        return newMatch;
    }
    public List<Match> BuildMany(int howMany)
    {
        var newMatches = new List<Match>();
        for (int i = 0; i < howMany; i++)
        {
            newMatches.Add(BuildOne());
        }
        return newMatches;
    }

    DateTime RandomDate(DateTime start, DateTime end)
    {
        var range = end - start;
        var randTicks = (long)(Rng.NextDouble() * range.Ticks);
        return start + TimeSpan.FromTicks(randTicks);
    }

    private MatchPlayerScore GeneratePlayerScore(Guid playerId)
    {
        var playerScore = new MatchPlayerScore();
        playerScore.PlayerId = playerId;
        playerScore.Score = Rng.NextInt64(_scoreRangeFrom, _scoreRangeTo);

        return playerScore;
    }
}
