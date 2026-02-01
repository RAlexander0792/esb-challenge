namespace ESBC.Domain.Entities;

public class EloRulesetSnapshot
{
    public string Algorithm { get; set; } = default!;
    public int K { get; set; }
    public double Tau { get; set; }
    //public bool ScaleKByPlayerCount { get; set; }
    //public double ScaleExponent { get; set; }
    //public long? MaxAbsScoreDiff { get; set; }
    //public bool RoundAtEnd { get; set; }
}
