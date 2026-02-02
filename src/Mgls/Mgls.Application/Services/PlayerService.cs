using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;

namespace Mgls.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILeaderboardPlayerRepository _leaderboardPlayerRepository;
    private readonly ISortedLeaderboardService _sortedLeaderboardService;
    public PlayerService(IPlayerRepository playerRepository,
                         ILeaderboardPlayerRepository leaderboardPlayerRepository,
                         ISortedLeaderboardService sortedLeaderboardService)
    {
        _playerRepository = playerRepository;
        _leaderboardPlayerRepository = leaderboardPlayerRepository;
        _sortedLeaderboardService = sortedLeaderboardService;
    }

    public async Task<Player> CreatePlayer(CreatePlayerDto playerDto, CancellationToken ct)
    {
        ValidateCreatePlayer(playerDto);

        var newPlayer = new Player()
        {
            DisplayName = playerDto.Name!,
            CreatedAt = DateTimeOffset.UtcNow,
        };
        await _playerRepository.InsertAsync(newPlayer, ct);

        return newPlayer;
    }

    public async Task<bool> DoesPlayerExists(Guid playerId, CancellationToken ct)
    {
        return await _playerRepository.GetByIdAsync(playerId, ct) != null;
    }

    public async Task<List<Player>> GetPlayers(int quantity, CancellationToken ct)
    {
        return await _playerRepository.GetPlayers(quantity, ct);
    }

    public async Task<PlayerProfileDto?> GetProfile(Guid playerId, CancellationToken ct)
    {
        var player = await _playerRepository.GetByIdAsync(playerId, ct);


        var playerRatings = await _leaderboardPlayerRepository.GetByPlayerId(playerId, ct);

        var playerProfile = new PlayerProfileDto();
        playerProfile.PlayerId = player.Id;
        playerProfile.DisplayName = player.DisplayName;
        playerProfile.CreatedAt = player.CreatedAt;

        foreach(var playerRating in playerRatings)
        {

            var position = await _sortedLeaderboardService.GetRank(playerRating.Id.LeaderboardId, playerRating.Id.PlayerId, ct);

            playerProfile.LeaderboardStats.Add(new PlayerLeaderboardStatsDto()
            {
                LeaderboardId = playerRating.Id.LeaderboardId,
                Losses = playerRating.Losses,
                Rating = playerRating.Rating,
                Wins = playerRating.Wins,
                Streak = playerRating.Streak,
                Position = position ?? 0
            });
        }


        return playerProfile;
    }

    public void ValidateCreatePlayer(CreatePlayerDto createPlayer)
    {
        if (string.IsNullOrWhiteSpace(createPlayer.Name))
        {
            throw new ArgumentException("Name can not be empty");
        }

        if (createPlayer.Name.Length < 3)
        {
            throw new ArgumentException("Name can must be at least 3 characters");
        }

        if (createPlayer.Name.Length > 32)
        {
            throw new ArgumentException("Name can not exceed 32 characters");
        }
    }
}
