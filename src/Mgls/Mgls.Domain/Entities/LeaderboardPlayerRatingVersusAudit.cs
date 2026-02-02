namespace Mgls.Domain.Entities;

public class LeaderboardPlayerRatingVersusAudit
{
    public Guid PlayerId { get; set; }

    public int RatingBefore { get; set; }
    public long Score { get; set; }
    public double Expected { get; set; }
    public double Actual { get; set; }       
    public double Delta { get; set; }

    public double RatingChangeContribution { get; set; }
}
