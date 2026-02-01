using Asp.Versioning;
using ESBC.API.Controllers.Dtos;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ESBC.API.Controllers;

[ApiVersion("1.0")]
public class MatchController : BaseAPIController
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
    {
        try
        {
            var match = await _matchService.CreateMatch(new CreateMatchDto
            {
                RankedBoardId = request.RankedBoardId,
                PlayerScores = request.PlayerScores
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
