using Mgls.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Mgls.Data.Documents;

public class LeaderboardPlayerRatingAuditDocument
{
    public static void Register()
    {
        RegisterKey();

        if (BsonClassMap.IsClassMapRegistered(typeof(LeaderboardPlayerRatingAudit)))
            return;

        BsonClassMap.RegisterClassMap<LeaderboardPlayerRatingAudit>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id);
            cm.MapMember(x => x.ActualTotal)
            .SetElementName("actualTotal");

            cm.MapMember(x => x.DeltaTotal)
            .SetElementName("deltaTotal");

            cm.MapMember(x => x.ExpectedTotal)
            .SetElementName("expectedTotal");

            cm.MapMember(x => x.RatingAfter)
            .SetElementName("ratingAfter");

            cm.MapMember(x => x.RatingBefore)
            .SetElementName("ratingBefore");

            cm.MapMember(x => x.RulesetSnapshot)
            .SetElementName("rulesetSnapshot");

            cm.MapMember(x => x.Versus)
            .SetElementName("versus");


            cm.SetIgnoreExtraElements(true);
        });
        RegisterNestedLeaderboardPlayerRatingVersusAudit();
    }

    private static void RegisterKey()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(LeaderboardPlayerRatingAuditId)))
            return;

        BsonClassMap.RegisterClassMap<LeaderboardPlayerRatingAuditId>(cm =>
        {
            cm.AutoMap();
            cm.MapMember(x => x.LeaderboardId)
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard))
            .SetElementName("leaderboardId");

            cm.MapMember(x => x.PlayerId)
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard))
            .SetElementName("playerId");

            cm.MapMember(x => x.MatchId)
            .SetSerializer(new GuidSerializer(GuidRepresentation.Standard))
            .SetElementName("matchId");

            cm.SetIgnoreExtraElements(true);
        });
    }

    private static void RegisterNestedLeaderboardPlayerRatingVersusAudit()
    {

        if (BsonClassMap.IsClassMapRegistered(typeof(LeaderboardPlayerRatingVersusAudit)))
            return;

        BsonClassMap.RegisterClassMap<LeaderboardPlayerRatingVersusAudit>(cm =>
        {
            cm.AutoMap();

            cm.MapMember(x => x.PlayerId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            cm.MapMember(x => x.Actual).SetElementName("actual");
            cm.MapMember(x => x.Delta).SetElementName("delta");
            cm.MapMember(x => x.Expected).SetElementName("expected");
            cm.MapMember(x => x.RatingBefore).SetElementName("ratingBefore");
            cm.MapMember(x => x.RatingChangeContribution).SetElementName("ratingChangeContribution");
            cm.MapMember(x => x.Score).SetElementName("score");

            cm.SetIgnoreExtraElements(true);
        });
    }

}
