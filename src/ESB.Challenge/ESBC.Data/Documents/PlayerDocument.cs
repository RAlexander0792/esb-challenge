using ESBC.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ESBC.Data.Documents;

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

            cm.MapMember(x => x.DisplayName).SetElementName("display_name");

            // defaults / rules
            cm.MapMember(x => x.CreatedAt)
              .SetDefaultValue(DateTime.UtcNow);

            cm.SetIgnoreExtraElements(true);
        });
    }

    public static void EnsureIndexes()
    {

    }
}
