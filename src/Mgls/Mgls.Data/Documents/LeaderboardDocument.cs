using Mgls.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Mgls.Data.Documents;

internal class LeaderboardDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Leaderboard)))
            return;

        BsonClassMap.RegisterClassMap<Leaderboard>(cm =>
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

            cm.MapMember(x => x.RatingRulesetId)
            .SetElementName("ratingRulesetId")
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));


            cm.SetIgnoreExtraElements(true);
        });
    }

}
