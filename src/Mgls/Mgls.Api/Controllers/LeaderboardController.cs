using Asp.Versioning;
using Mgls.API.Controllers.Dtos;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mgls.API.Controllers;

[ApiVersion("1.0")]
public class LeaderboardController : BaseAPIController
{
    private readonly ILeaderboardService _leaderboardService;
    private readonly ISortedLeaderboardService _sortedLeaderboardService;


    public LeaderboardController(ILeaderboardService leaderboardService,
                                 ISortedLeaderboardService sortedLeaderboardService)
    {
        _leaderboardService = leaderboardService;
        _sortedLeaderboardService = sortedLeaderboardService;
    }

    [HttpGet("{leaderboardId}")]
    public async Task<IActionResult> GetLeaderboardById([FromRoute] Guid leaderboardId, CancellationToken ct)
    {
        var leaderboard = await _leaderboardService.GetById(leaderboardId, ct);
        return leaderboard != null ? Ok(leaderboard) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateLeaderboard([FromBody] CreateLeaderboardRequest request, CancellationToken ct)
    {
        try
        {
            var leaderboard = await _leaderboardService.CreateLeaderboard(new CreateLeaderboardDto
            {
                Name = request.Name,
                Description = request.Description,
                RatingRulesetId = request.RatingRulesetId
            }, ct);

            return CreatedAtAction(nameof(GetLeaderboardById), new { leaderboardId = leaderboard.Id }, leaderboard);
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

    [HttpGet("{leaderboardId}/player/top")]
    public async Task<IActionResult> GetTopPlayers([FromRoute]Guid leaderboardId, CancellationToken ct, [FromQuery] int skip = 0, [FromQuery]int limit = 100)
    {
        try
        {
            var players = await _leaderboardService.GetTopPlayers(leaderboardId, skip,limit);

            return Ok(players);
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

    [HttpGet("{leaderboardId}/match/history")]
    public async Task<IActionResult> GetLeaderboardMatchHistory([FromRoute] Guid leaderboardId, CancellationToken ct, [FromQuery] int limit = 100)
    {
        try
        {
            var matches = await _leaderboardService.GetLeaderboardMatchHistory(leaderboardId, limit, ct);

            return Ok(matches);
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

    [HttpPost("{leaderboardId}/load-cache")]
    public async Task<IActionResult> LoadLeaderboardPlayerCache([FromRoute] Guid leaderboardId)
    {
        await _sortedLeaderboardService.RebuildLeaderboardCacheAsync(leaderboardId);
        return Ok();
    }
}
