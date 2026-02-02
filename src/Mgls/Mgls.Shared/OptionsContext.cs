namespace Mgls.Shared;

public class OptionsContext
{
    public FeatureFlags Features { get; private set; }

    public string DbConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public RedisOptions Redis { get; set; }
}

public class RedisOptions
{
    public string ConnectionString { get; set; }
    public string InstancePrefix { get; set; }
}
