using Mgls.Domain.Entities;

namespace Mgls.Benchmarks.EntityBuilders;

public class PlayerBuilder
{
    private readonly Random Rng;
    private long ratingFrom;
    private long ratingTo;


    public PlayerBuilder(int seed)
    {
        Rng = new Random(seed);
    }

    public PlayerBuilder WithRatingRange(long ratingFrom, long ratingTo)
    {
        this.ratingFrom = ratingFrom;
        this.ratingTo = ratingTo;
        return this;
    }

    public LeaderboardPlayer BuildOne()
    {
        return new LeaderboardPlayer()
        {
            Rating = Rng.Next((int)ratingFrom, (int)ratingTo),
            Id = new LeaderboardPlayerId(Guid.NewGuid(), Guid.NewGuid())
        };
    }

    public List<LeaderboardPlayer> BuildMany(int howMany)
    {
        var player = new List<LeaderboardPlayer>();

        for (int i = 0; i < howMany; i++)
        {
            player.Add(BuildOne());
        }
        return player;
    }
}
