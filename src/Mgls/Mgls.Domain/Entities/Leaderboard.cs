namespace Mgls.Domain.Entities;

public class Leaderboard
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int GameMode { get; set; }
    public Guid RatingRulesetId { get; set; }
}
