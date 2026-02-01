namespace ESBC.Domain.Entities;

public class PlayerEloAuditVersus
{
    public Guid OpponentPlayerId { get; set; }

    public int OpponentEloBefore { get; set; }
    public long OpponentScore { get; set; }

    public long ScoreDiff { get; set; }        // PlayerScore - OpponentScore

    public double Expected { get; set; }       // rating-based
    public double Actual { get; set; }         // margin-based
    public double Delta { get; set; }          // Actual - Expected

    public double RatingChangeContribution { get; set; }
}
