using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using Mgls.Shared;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Mgls.Application.Services;

public class SortedLeaderboardService : ISortedLeaderboardService
{
    private readonly IConnectionMultiplexer _mux;
    private readonly RedisOptions _opt;

    public SortedLeaderboardService(IConnectionMultiplexer mux, IOptions<OptionsContext> opt)
    {
        _mux = mux;
        _opt = opt.Value.Redis;
    }

    private IDatabase Db => _mux.GetDatabase();

    private RedisKey Key(Guid leaderboardId) => $"{_opt.InstancePrefix}leaderboard:{leaderboardId:N}";

    public async Task Update(Guid leaderboardId, Guid playerId, int rating, CancellationToken ct = default)
    {
        await Db.SortedSetAddAsync(Key(leaderboardId), playerId.ToString("N"), rating);
    }

    public async Task<long?> GetRank(Guid leaderboardId, Guid playerId, CancellationToken ct = default)
    {
        // Descending rank: highest rating = rank 1
        var zeroBased = await Db.SortedSetRankAsync(
            Key(leaderboardId),
            playerId.ToString("N"),
            Order.Descending);

        return zeroBased.HasValue ? zeroBased.Value + 1 : null;
    }

    public async Task<List<PlayerLeaderboardRankDto>> GetTopPlayerLeadboardStats(Guid leaderboardId, int skip, int take)
    {
        if (take <= 0) return Array.Empty<PlayerLeaderboardRankDto>().ToList();

        var entries = await Db.SortedSetRangeByRankWithScoresAsync(
            Key(leaderboardId),
            start: skip,
            stop: take - 1,
            order: Order.Descending);

        var result = new List<PlayerLeaderboardRankDto>(entries.Length);
        var i = 1;
        foreach (var e in entries)
        {
            if (!e.Element.HasValue) continue;
            if (!Guid.TryParseExact(e.Element.ToString(), "N", out var playerId)) continue;
            result.Add(new PlayerLeaderboardRankDto() { PlayerId = playerId, Rating = (int)e.Score, Position = skip + i });
            i++;
        }

        return result;
    }
    
}
