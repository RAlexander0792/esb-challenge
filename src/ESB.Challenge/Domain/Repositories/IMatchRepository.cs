using ESBC.Domain.Entities;

namespace ESBC.Domain.Repositories;

public interface IMatchRepository
{
    public Task<Match?> GetByIdAsync(Guid id, CancellationToken ct);
    public Task InsertAsync(Match doc, CancellationToken ct);
}
