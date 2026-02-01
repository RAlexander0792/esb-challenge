using ESBC.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ESBC.Data.Documents;

internal class MatchDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Match)))
            return;

        BsonClassMap.RegisterClassMap<Match>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id)
             .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.MapMember(x => x.RankedBoardId)
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard)).SetElementName("rankedBoardId");

            cm.MapMember(x => x.PlayerScores)
            .SetElementName("playerScores");

            cm.MapMember(x => x.CreatedAt)
              .SetDefaultValue(DateTime.UtcNow)
              .SetElementName("createdAt");

            cm.MapMember(x => x.EndedAt)
              .SetElementName("endedAt");

            cm.MapMember(x => x.StartedAt)
              .SetElementName("startedAt");

            cm.SetIgnoreExtraElements(true);
        });

        RegisterNestedMatchPlayerScore();

    }

    private static void RegisterNestedMatchPlayerScore()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(MatchPlayerScore)))
            return;

        BsonClassMap.RegisterClassMap<MatchPlayerScore>(cm =>
        {
            cm.AutoMap();

            cm.MapMember(x => x.PlayerId)
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard))
            .SetElementName("playerId");

            cm.MapMember(x => x.Score)
            .SetElementName("score");

            cm.SetIgnoreExtraElements(true);
        });
    }

    public static void EnsureIndexes()
    {

    }
}
