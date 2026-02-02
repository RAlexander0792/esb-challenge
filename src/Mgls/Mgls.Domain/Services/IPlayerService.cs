using Mgls.Domain.Entities;
using Mgls.Domain.Services.Dtos;

namespace Mgls.Domain.Services;

public interface IPlayerService
{
    Task<Player> CreatePlayer(CreatePlayerDto player, CancellationToken ct);
    Task<bool> DoesPlayerExists(Guid playerId, CancellationToken ct);
    Task<List<Player>> GetPlayers(int quantity, CancellationToken ct);
    Task<PlayerProfileDto> GetProfile(Guid playerId, CancellationToken ct);
}
