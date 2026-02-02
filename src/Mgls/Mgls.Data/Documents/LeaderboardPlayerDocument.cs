using Mgls.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Mgls.Data.Documents;

public class LeaderboardPlayerDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(LeaderboardPlayerId)))
            return;

        BsonClassMap.RegisterClassMap<LeaderboardPlayerId>(cm =>
        {
            cm.AutoMap();

            cm.MapMember(x => x.PlayerId)
            .SetElementName("playerId")
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.MapMember(x => x.LeaderboardId)
            .SetElementName("leaderboardId")
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });

        if (BsonClassMap.IsClassMapRegistered(typeof(LeaderboardPlayer)))
            return;

        BsonClassMap.RegisterClassMap<LeaderboardPlayer>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id);

            cm.MapMember(x => x.Wins)
            .SetElementName("wins");

            cm.MapMember(x => x.Losses)
            .SetElementName("losses");

            cm.MapMember(x => x.Streak)
            .SetElementName("streak");

            cm.MapMember(x => x.Rating)
            .SetElementName("rating");

            cm.MapMember(x => x.LastAppliedMatchId)
            .SetElementName("lastAppliedMatchId")
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.SetIgnoreExtraElements(true);
        });
    }

}
