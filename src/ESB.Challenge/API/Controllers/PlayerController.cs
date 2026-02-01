using Asp.Versioning;
using ESBC.API.Controllers.Dtos;
using ESBC.Domain.Services;
using ESBC.Domain.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ESBC.API.Controllers;

[ApiVersion("1.0")]
public class PlayerController : BaseAPIController
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        try
        {
            ValidateCreatePlayerRequest(request);

            var player = await _playerService.CreatePlayer(new CreatePlayerDto { Name = request.Name });

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
}
