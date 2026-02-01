namespace ESBC.Data;

public sealed class MongoOptions
{
    public string ConnectionString { get; init; } = default!;
    public string DatabaseName { get; init; } = default!;
}
