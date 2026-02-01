using ESBC.Domain.Entities;

namespace ESBC.Benchmarks.EntityBuilders;

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

    public RankedBoardPlayer BuildOne()
    {
        return new RankedBoardPlayer()
        {
            ELO = Rng.Next((int)ratingFrom, (int)ratingTo),
            Id = new RankedBoardPlayerId(Guid.NewGuid(), Guid.NewGuid())
        };
    }

    public List<RankedBoardPlayer> BuildMany(int howMany)
    {
        var player = new List<RankedBoardPlayer>();

        for (int i = 0; i < howMany; i++)
        {
            player.Add(BuildOne());
        }
        return player;
    }
}
