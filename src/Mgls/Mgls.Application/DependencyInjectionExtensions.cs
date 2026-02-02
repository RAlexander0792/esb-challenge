using Mgls.Application.BackgroundServices;
using Mgls.Application.Factories;
using Mgls.Application.Services;
using Mgls.Domain.Entities;
using Mgls.Domain.Services;
using Mgls.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Threading.Channels;

namespace Mgls.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IPlayerService, PlayerService>();
        services.AddTransient<IMatchService, MatchService>();
        services.AddTransient<ILeaderboardService, LeaderboardService>();
        services.AddTransient<IRatingRulesetService, RatingRulesetService>();
        services.AddTransient<ILeaderboardPlayerRatingAuditService, PlayerRatingAuditService>();

        services.AddSingleton<IRatingResolverFactory, RatingResolverFactory>();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var opt = sp.GetRequiredService<IOptions<OptionsContext>>().Value;

            var config = ConfigurationOptions.Parse(opt.Redis.ConnectionString);
            config.AbortOnConnectFail = false;

            return ConnectionMultiplexer.Connect(config);
        });

        services.AddSingleton<ISortedLeaderboardService, SortedLeaderboardService>();

        return services;
    }


    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {

        services.AddSingleton(Channel.CreateBounded<Match>(
        new BoundedChannelOptions(capacity: 10_000)
        {
            SingleReader = true,   
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        }));


        services.AddHostedService<MatchRatingProcessor>();



        return services;
    }
}
