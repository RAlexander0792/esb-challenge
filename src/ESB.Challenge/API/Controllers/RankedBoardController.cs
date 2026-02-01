using Asp.Versioning;
using ESBC.API.Controllers.Dtos;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ESBC.API.Controllers;

[ApiVersion("1.0")]
public class RankedBoardController : BaseAPIController
{
    private readonly IRankedBoardService _rankedBoardService;

    public RankedBoardController(IRankedBoardService rankedBoardService)
    {
        _rankedBoardService = rankedBoardService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRankedBoard([FromBody] CreateRankedBoardRequest request)
    {
        try
        {
            var player = await _rankedBoardService.CreateRankedBoard(new CreateRankedBoardDto { Name = request.Name });

            return CreatedAtAction("CreateRankedBoard", player.Id);
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

    [HttpGet("{rankedBoardId}")]
    public async Task<IActionResult> GetTopPlayers([FromRoute]Guid rankedBoardId, [FromQuery]int limit = 100)
    {

        try
        {
            var players = await _rankedBoardService.GetTopPlayers(rankedBoardId, limit);

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
}
