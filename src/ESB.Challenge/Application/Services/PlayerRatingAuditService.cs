using ESBC.Domain.Entities;
using ESBC.Domain.Repositories;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;

namespace ESBC.Application.Services;

public class PlayerRatingAuditService : IPlayerEloAuditService
{
    private readonly IPlayerEloAuditRepository _playerEloAuditRepository;

    public PlayerRatingAuditService(IPlayerEloAuditRepository playerEloAuditRepository)
    {
        _playerEloAuditRepository = playerEloAuditRepository;
    }

    public async Task<bool> BatchInsertPlayerRatingAudit(CreatePlayerEloAuditDto batchInsertPlayerRatingAudit)
    {

        foreach(var record in batchInsertPlayerRatingAudit.Payload)
        {
            await _playerEloAuditRepository.InsertAsync(record, new CancellationToken());
        }

        return true;
    }
}
