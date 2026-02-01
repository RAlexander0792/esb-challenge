namespace ESBC.Core;

public class OptionsContext
{
    public FeatureFlags Features { get; private set; }

    public string DbConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
