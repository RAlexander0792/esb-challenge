using Mgls.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Mgls.Data.Documents;

internal class RatingRulesetDocument
{
    public static void Register()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(RatingRuleset)))
            return;

        BsonClassMap.RegisterClassMap<RatingRuleset>(cm =>
        {
            cm.AutoMap();

            cm.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

            cm.MapMember(x => x.Name).SetElementName("name");
            cm.MapMember(x => x.Resolver).SetElementName("resolver");
            cm.MapMember(x => x.K).SetElementName("k");
            cm.MapMember(x => x.Tau).SetElementName("tau");

            cm.SetIgnoreExtraElements(true);
        });

    }


}
