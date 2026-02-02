namespace Mgls.Domain.Services.Dtos;

public class CreateLeaderboardDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid RatingRulesetId { get; set; }
}

public class LeaderboardTopPlayersDto()
{
    public Guid LeaderboardId { get; set; }
    public string LeaderboardName { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public List<LeaderboardPlayerProfile> Players { get; set; } = new List<LeaderboardPlayerProfile>();
}

public class LeaderboardPlayerProfile
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; }
    public int Rating { get; set; }
    public int Position { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Streak { get; set; }
}