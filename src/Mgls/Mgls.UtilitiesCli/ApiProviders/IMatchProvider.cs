using Mgls.API.Controllers.Dtos;
using Refit;

namespace Mgls.UtilitiesCli.ApiProviders;

public interface IMatchProvider
{
    [Post("/match/result")]
    Task<HttpResponseMessage> PostMatch([Body] CreateMatchRequest request);
}
