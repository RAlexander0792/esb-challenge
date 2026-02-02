using Mgls.Domain.Entities;
using Mgls.Domain.Repositories;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;

namespace Mgls.Application.Services;

public class PlayerRatingAuditService : Domain.Services.ILeaderboardPlayerRatingAuditService
{
    private readonly Domain.Repositories.ILeaderboardPlayerRatingAuditRepository _playerEloAuditRepository;

    public PlayerRatingAuditService(Domain.Repositories.ILeaderboardPlayerRatingAuditRepository playerEloAuditRepository)
    {
        _playerEloAuditRepository = playerEloAuditRepository;
    }

    public async Task<bool> BatchInsertPlayerRatingAudit(CreateLeaderboardPlayerRatingAuditsDto batchInsertPlayerRatingAudit)
    {

        foreach(var record in batchInsertPlayerRatingAudit.Payload)
        {
            await _playerEloAuditRepository.InsertAsync(record, new CancellationToken());
        }

        return true;
    }
}
