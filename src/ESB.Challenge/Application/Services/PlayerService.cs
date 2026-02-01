using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;

namespace ESBC.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Player> CreatePlayer(CreatePlayerDto playerDto)
    {
        ValidateCreatePlayer(playerDto);

        var newPlayer = new Player()
        {
            DisplayName = playerDto.Name!,
            CreatedAt = DateTimeOffset.UtcNow,
        };
        await _playerRepository.InsertAsync(newPlayer, new CancellationToken());

        return newPlayer;
    }

    public async Task<bool> DoesPlayerExists(Guid playerId)
    {
        return await _playerRepository.GetByIdAsync(playerId, CancellationToken.None) != null;
    }

    public async Task<List<Player>> GetPlayers(int quantity)
    {
        return await _playerRepository.GetPlayers(quantity);
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
