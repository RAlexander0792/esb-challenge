using ESBC.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ESBC.Data.Documents;

internal class RankedBoardDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(RankedBoard)))
            return;

        BsonClassMap.RegisterClassMap<RankedBoard>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id)
             .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.MapMember(x => x.Name)
            .SetElementName("name");

            cm.MapMember(x => x.Description)
            .SetElementName("description");

            cm.MapMember(x => x.GameMode)
            .SetElementName("gameMode");

            cm.MapMember(x => x.RulesetVersion)
            .SetElementName("rulesetVersion");


            cm.SetIgnoreExtraElements(true);
        });
    }

    public static void EnsureIndexes()
    {

    }
}
