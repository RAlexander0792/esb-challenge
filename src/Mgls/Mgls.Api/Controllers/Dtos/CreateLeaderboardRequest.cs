namespace Mgls.API.Controllers.Dtos;

public class CreateLeaderboardRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid RatingRulesetId { get; set; }
}
