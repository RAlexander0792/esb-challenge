using Mgls.Domain.Entities;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using Mgls.Shared;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Mgls.Application.Services;

public class SortedLeaderboardService : ISortedLeaderboardService
{
    private readonly IConnectionMultiplexer _mux;
    private readonly RedisOptions _opt;
    private readonly IMongoCollection<LeaderboardPlayer> _leaderboardPlayers;

    public SortedLeaderboardService(IConnectionMultiplexer mux,
                                    IOptions<OptionsContext> opt,
                                    IMongoCollection<LeaderboardPlayer> leaderboardPlayers)
    {
        _mux = mux;
        _opt = opt.Value.Redis;
        _leaderboardPlayers = leaderboardPlayers;
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

    public async Task RebuildLeaderboardCacheAsync(Guid boardId, CancellationToken ct = default)
    {
        var key = Key(boardId);
        await Db.KeyDeleteAsync(key);

        var filter = Builders<LeaderboardPlayer>.Filter.Eq(x => x.Id.LeaderboardId, boardId);


        const int mongoBatchSize = 5_000;
        const int redisChunkSize = 2_000;

        var options = new FindOptions<LeaderboardPlayer, PlayerLeaderboardRankDto>
        {
            BatchSize = mongoBatchSize,
            Projection = Builders<LeaderboardPlayer>.Projection.Expression(
                x =>  new PlayerLeaderboardRankDto() { PlayerId = x.Id.PlayerId, Rating = x.Rating}
            )
        };

        using var cursor = await _leaderboardPlayers.FindAsync(filter, options, ct);

        var buffer = new List<(Guid PlayerId, double Rating)>(redisChunkSize);

        while (await cursor.MoveNextAsync(ct))
        {
            foreach (var doc in cursor.Current)
            {
                buffer.Add((doc.PlayerId, doc.Rating));

                if (buffer.Count >= redisChunkSize)
                {
                    await FlushToRedisAsync(key, buffer);
                    buffer.Clear();
                }
            }
        }

        if (buffer.Count > 0)
        {
            await FlushToRedisAsync(key, buffer);
            buffer.Clear();
        }
    }

    private async Task FlushToRedisAsync(RedisKey key, List<(Guid PlayerId, double Rating)> items)
    {
        var batch = Db.CreateBatch();
        var tasks = new Task[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            var (playerId, rating) = items[i];
            tasks[i] = batch.SortedSetAddAsync(key, playerId.ToString("N"), rating);
        }

        batch.Execute();
        await Task.WhenAll(tasks);
    }
}

