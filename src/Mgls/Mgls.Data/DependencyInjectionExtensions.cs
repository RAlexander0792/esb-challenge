using Mgls.Data.Documents;
using Mgls.Data.Repositories;
using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using Mgls.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Mgls.Data;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<OptionsContext>>().Value;
            return new MongoClient(opt.DbConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<OptionsContext>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(opt.DatabaseName);
        });

        // Collection registration
        services.AddSingleton<IMongoCollection<Player>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<Player>("players");
        });

        services.AddSingleton<IMongoCollection<LeaderboardPlayerRatingAudit>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<LeaderboardPlayerRatingAudit>("leaderboardPlayerRatingAudits");
        });

        services.AddSingleton<IMongoCollection<Leaderboard>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<Leaderboard>("leaderboards");
        });

        services.AddSingleton<IMongoCollection<LeaderboardPlayer>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<LeaderboardPlayer>("leaderboardPlayers");
        });

        services.AddSingleton<IMongoCollection<Match>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<Match>("matches");
        });

        services.AddSingleton<IMongoCollection<RatingRuleset>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<RatingRuleset>("ratingRulesets");
        });

        PlayerDocument.Register();
        MatchDocument.Register();
        RatingRulesetDocument.Register();
        LeaderboardDocument.Register();
        LeaderboardPlayerDocument.Register();
        LeaderboardPlayerRatingAuditDocument.Register();

        // Repository
        services.AddTransient<IPlayerRepository, PlayerRepository>();
        services.AddTransient<IMatchRepository, MatchRepository>();
        services.AddTransient<IRatingRulesetRepository, RatingRulesetRepository>();
        services.AddTransient<ILeaderboardRepository, LeaderboardRepository>();
        services.AddTransient<ILeaderboardPlayerRepository, LeaderboardPlayerRepository>();
        services.AddTransient<ILeaderboardPlayerRatingAuditRepository, LeaderboardPlayerRatingAuditRepository>();

        return services;
    }

}
