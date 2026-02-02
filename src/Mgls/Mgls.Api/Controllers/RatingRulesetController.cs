using Asp.Versioning;
using Mgls.Api.Controllers.Dtos;
using Mgls.Application.Services.RatingResolvers;
using Mgls.Domain.Entities;
using Mgls.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mgls.API.Controllers;

[ApiVersion("1.0")]
public class RatingRulesetController : BaseAPIController
{
    // A better way of exposing the available resolvers and their capabilities could be done.
    private readonly List<ResolverDescriptionResponse> _resolvers = new List<ResolverDescriptionResponse>
    {
        new ResolverDescriptionResponse
        {
            Name = nameof(ClassicEloResolver),
            Description = "2 Players support only. Score is interpreted as greater number won, same numbers draw"
        },
        new ResolverDescriptionResponse
        {
            Name = nameof(EloPlusResolver),
            Description = "N Players Free for all support. Score difference is used to determine decisiveness, scaled by Tau. K rating is scaled for number of players"
        }
    };

    private readonly IRatingRulesetService _ratingRulesetService;

    public RatingRulesetController(IRatingRulesetService ratingRulesetService)
    {
        _ratingRulesetService = ratingRulesetService;
    }


    [HttpGet("{ratingRulesetId}")]
    public async Task<IActionResult> GetRatingRuleset([FromRoute] Guid ratingRulesetId, CancellationToken ct)
    {
        try
        {
            var ratingRuleset = await _ratingRulesetService.GetById(ratingRulesetId, ct);
            
            return ratingRuleset != null ? Ok(ratingRuleset) : NotFound();
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

    [HttpPost]
    public async Task<IActionResult> CreateRatingRuleset([FromBody] RatingRuleset request)
    {
        try
        {
            if (!_resolvers.Any(x => x.Name == request.Resolver))
                return BadRequest("Invalid Resolver");

            var ratingRuleset = await _ratingRulesetService.Create(request);

            return CreatedAtAction(actionName: "GetRatingRuleset", routeValues: new { ratingRulesetId = ratingRuleset.Id }, value: ratingRuleset);
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

    [HttpGet("resolvers")]
    public IActionResult GetRatingResolverNames()
    {
        return Ok(_resolvers);
    }

 
}
