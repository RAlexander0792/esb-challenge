using ESBC.Core;
using ESBC.Data.Documents;
using ESBC.Data.Repositories;
using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ESBC.Data;

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

        services.AddSingleton<IMongoCollection<RankedBoardPlayerRatingAudit>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<RankedBoardPlayerRatingAudit>("playerEloAudits");
        });

        services.AddSingleton<IMongoCollection<RankedBoard>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<RankedBoard>("rankedBoards");
        });

        services.AddSingleton<IMongoCollection<RankedBoardPlayer>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<RankedBoardPlayer>("rankedBoardPlayers");
        });

        services.AddSingleton<IMongoCollection<Match>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<Match>("matches");
        });

        PlayerDocument.Register();
        MatchDocument.Register();
        RankedBoardDocument.Register();
        RankedBoardPlayerDocument.Register();
        PlayerEloAuditDocument.Register();

        // Repository
        services.AddTransient<IPlayerRepository, PlayerRepository>();
        services.AddTransient<IMatchRepository, MatchRepository>();
        services.AddTransient<IRankedBoardRepository, RankedBoardRepository>();
        services.AddTransient<IRankedBoardPlayerRepository, RankedBoardPlayerRepository>();
        services.AddTransient<IPlayerEloAuditRepository, PlayerEloAuditRepository>();




        return services;
    }
}
