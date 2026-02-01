using ESBC.API.Controllers.Dtos;
using ESBC.Domain.Entities;
using Refit;

namespace ESBC.UtilitiesCli.ApiProviders;

public interface IPlayerProvider
{
    [Post("/Player")]
    Task<HttpResponseMessage> PostPlayer([Body] CreatePlayerRequest request);
}
