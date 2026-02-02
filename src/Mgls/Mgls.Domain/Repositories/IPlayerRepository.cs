using Mgls.Domain.Entities;

namespace Mgls.Domain.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Player>> GetPlayers(int quantity, CancellationToken ct);
    Task InsertAsync(Player doc, CancellationToken ct);
}
