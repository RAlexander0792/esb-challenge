using Mgls.API.Controllers.Dtos;
using Refit;

namespace Mgls.UtilitiesCli.ApiProviders;

public interface IPlayerProvider
{
    [Post("/player")]
    Task<HttpResponseMessage> PostPlayer([Body] CreatePlayerRequest request);
}
