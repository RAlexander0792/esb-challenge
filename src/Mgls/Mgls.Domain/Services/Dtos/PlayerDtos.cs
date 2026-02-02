namespace Mgls.Domain.Services.Dtos;

public class CreatePlayerDto
{
    public string? Name { get; set; }
}

public class PlayerProfileDto
{
    public Guid PlayerId { get; set; }
    public string DisplayName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<PlayerLeaderboardStatsDto> LeaderboardStats { get; set; } = [];
}

public class PlayerLeaderboardRankDto()
{
    public Guid PlayerId { get; set; }
    public int Position { get; set; }
    public int Rating { get; set; }
}

public class PlayerLeaderboardStatsDto()
{
    public Guid LeaderboardId { get; set; }
    public int Rating { get; set; }
    public long Position { get; set; }
    public int Streak { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
}