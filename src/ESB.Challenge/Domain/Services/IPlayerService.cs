using ESBC.Domain.Entities;
using ESBC.Domain.Services.Dtos;

namespace ESBC.Domain.Services;

public interface IPlayerService
{
    Task<Player> CreatePlayer(CreatePlayerDto player);
    Task<bool> DoesPlayerExists(Guid playerId);
    Task<List<Player>> GetPlayers(int quantity);
}
