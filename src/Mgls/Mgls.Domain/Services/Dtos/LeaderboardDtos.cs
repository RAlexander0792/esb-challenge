namespace Mgls.Domain.Services.Dtos;

public class CreateLeaderboardDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid RatingRulesetId { get; set; }
}
