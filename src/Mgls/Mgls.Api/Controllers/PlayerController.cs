using Asp.Versioning;
using Mgls.API.Controllers.Dtos;
using Mgls.Domain.Services;
using Mgls.Domain.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Mgls.API.Controllers;

[ApiVersion("1.0")]
public class PlayerController : BaseAPIController
{
    private readonly IPlayerService _playerService;
    private readonly IMatchService _matchService;

    public PlayerController(IPlayerService playerService,
                            IMatchService matchService)
    {
        _playerService = playerService;
        _matchService = matchService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request, CancellationToken ct)
    {
        try
        {
            ValidateCreatePlayerRequest(request);

            var player = await _playerService.CreatePlayer(new CreatePlayerDto { Name = request.Name }, ct);

            return CreatedAtAction("CreatePlayer", player.Id);
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
    private void ValidateCreatePlayerRequest(CreatePlayerRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new ArgumentException("Name can not be empty");
        }
    }

    [HttpGet("{playerId}/profile")]
    public async Task<IActionResult> GetPlayerProfile([FromRoute] Guid playerId, CancellationToken ct)
    {
        try
        {
            return Ok(await _playerService.GetProfile(playerId, ct));
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

    [HttpGet("{playerId}/match/history")]
    public async Task<IActionResult> GetPlayerMatchHistory([FromRoute] Guid playerId, CancellationToken ct)
    {
        try
        {
            return Ok(await _matchService.GetByPlayer(playerId, ct));
            
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
