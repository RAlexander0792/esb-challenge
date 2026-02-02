using Mgls.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Mgls.Data.Documents;

internal class PlayerDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Player)))
            return;

        BsonClassMap.RegisterClassMap<Player>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id)
             .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.MapMember(x => x.DisplayName)
            .SetElementName("display_name");

            cm.MapMember(x => x.CreatedAt)
              .SetDefaultValue(DateTime.UtcNow);

            cm.SetIgnoreExtraElements(true);
        });
    }

    public static async Task EnsureIndexes(IMongoCollection<Player> collection)
    {
        var indexes = new[]
        {
            new CreateIndexModel<Player>(
                Builders<Player>.IndexKeys
                    .Ascending(x => x.CreatedAt),
                new CreateIndexOptions { Name = "ix_player_createdAt" }
            )
        };

        await collection.Indexes.CreateManyAsync(indexes);
    }
}
