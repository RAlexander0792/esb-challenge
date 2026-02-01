namespace ESBC.Domain.Entities;

public class RankedBoard
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int GameMode { get; set; }
    public int RulesetVersion { get; set; }
}
