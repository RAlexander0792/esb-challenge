using ESBC.Domain.Entities;

namespace ESBC.Domain.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Player>> GetPlayers(int quantity);
    Task InsertAsync(Player doc, CancellationToken ct);
}
