using Asp.Versioning;
using Mgls.API.Controllers.Dtos;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mgls.API.Controllers;

[ApiVersion("1.0")]
public class MatchController : BaseAPIController
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpPost("result")]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
    {
        try
        {
            var match = await _matchService.Create(new CreateMatchDto
            {
                LeaderboardId = request.LeaderboardId,
                PlayerScores = request.PlayerScores,
                StartedAt = request.StartedAt,
                CreatedAt = request.CreatedAt,
                EndedAt = DateTimeOffset.UtcNow
            });

            return CreatedAtAction("CreateMatch", match.Id);
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException)
            {
                return BadRequest(ex.Message);
            }
            return StatusCode(500, ex.Message);
        }
    }

 
}
