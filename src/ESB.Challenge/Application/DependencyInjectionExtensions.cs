using ESBC.Application.BackgroundServices;
using ESBC.Application.Factories;
using ESBC.Application.Services;
using ESBC.Domain.Entities;
using ESBC.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

namespace ESBC.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IPlayerService, PlayerService>();
        services.AddTransient<IMatchService, MatchService>();
        services.AddTransient<IRankedBoardService, RankedBoardService>();
        services.AddTransient<IPlayerEloAuditService, PlayerRatingAuditService>();

        services.AddSingleton<IEloResolverFactory, EloResolverFactory>();



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
