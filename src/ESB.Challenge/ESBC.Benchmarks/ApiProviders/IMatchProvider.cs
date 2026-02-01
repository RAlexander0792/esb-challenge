using ESBC.API.Controllers.Dtos;
using Refit;

namespace ESBC.UtilitiesCli.ApiProviders;

public interface IMatchProvider
{
    [Post("/Match")]
    Task<HttpResponseMessage> PostMatch([Body] CreateMatchRequest request);
}
