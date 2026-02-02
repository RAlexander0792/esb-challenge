using Mgls.Data.Documents;
using Mgls.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Mgls.Data;

public class DBIndexRegister
{
    public static async Task EnsureIX(IServiceProvider serviceProvider)
    {
        await PlayerDocument.EnsureIndexes(serviceProvider.GetRequiredService<IMongoCollection<Player>>());
        await MatchDocument.EnsureIndexes(serviceProvider.GetRequiredService<IMongoCollection<Match>>());
    }
}
