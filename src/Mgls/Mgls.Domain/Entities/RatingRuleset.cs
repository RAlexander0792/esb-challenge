namespace Mgls.Domain.Entities;

public class RatingRuleset
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Resolver { get; set; } = default!;
    public int K { get; set; }
    public double Tau { get; set; }
}
