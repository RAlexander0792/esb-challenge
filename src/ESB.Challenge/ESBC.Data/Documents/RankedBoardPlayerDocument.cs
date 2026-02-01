using ESBC.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ESBC.Data.Documents;

public class RankedBoardPlayerDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(RankedBoardPlayerId)))
            return;
        BsonClassMap.RegisterClassMap<RankedBoardPlayerId>(cm =>
        {
            cm.AutoMap();

            cm.MapMember(x => x.PlayerId).SetElementName("playerId").SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            cm.MapMember(x => x.RankedBoardId).SetElementName("rankedBoardId").SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });

        if (BsonClassMap.IsClassMapRegistered(typeof(RankedBoardPlayer)))
            return;

        BsonClassMap.RegisterClassMap<RankedBoardPlayer>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id);

            cm.MapMember(x => x.LastAppliedMatchId).SetElementName("lastAppliedMatchId")
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.SetIgnoreExtraElements(true);
        });
    }
}
