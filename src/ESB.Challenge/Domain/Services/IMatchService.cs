using ESBC.Domain.Entities;
using ESBC.Domain.Services.Dtos;

namespace ESBC.Domain.Services;

public interface IMatchService
{
    Task<Match> CreateMatch(CreateMatchDto match);
}
