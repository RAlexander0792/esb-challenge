using ESBC.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ESBC.Data.Documents;

public class PlayerEloAuditDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(RankedBoardPlayerRatingAuditId)))
            return;

        BsonClassMap.RegisterClassMap<RankedBoardPlayerRatingAuditId>(cm =>
        {
            cm.AutoMap();
            cm.MapMember(x => x.RankedBoardId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            cm.MapMember(x => x.PlayerId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            cm.MapMember(x => x.MatchId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.SetIgnoreExtraElements(true);
        });

        if (BsonClassMap.IsClassMapRegistered(typeof(RankedBoardPlayerRatingAudit)))
            return;

        BsonClassMap.RegisterClassMap<RankedBoardPlayerRatingAudit>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id);

            cm.SetIgnoreExtraElements(true);
        });
        RegisterNestedPlayerEloAuditVersus();
        RegisterNestedEloRulesetSnapshot();
    }

    private static void RegisterNestedEloRulesetSnapshot()
    {

        if (BsonClassMap.IsClassMapRegistered(typeof(EloRulesetSnapshot)))
            return;

        BsonClassMap.RegisterClassMap<EloRulesetSnapshot>(cm =>
        {
            cm.AutoMap();


            cm.SetIgnoreExtraElements(true);
        });
    }

    private static void RegisterNestedPlayerEloAuditVersus()
    {

        if (BsonClassMap.IsClassMapRegistered(typeof(PlayerEloAuditVersus)))
            return;

        BsonClassMap.RegisterClassMap<PlayerEloAuditVersus>(cm =>
        {
            cm.AutoMap();

            cm.MapMember(x => x.OpponentPlayerId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));


            cm.SetIgnoreExtraElements(true);
        });
    }
}
