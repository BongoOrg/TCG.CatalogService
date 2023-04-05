using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.CatalogService.Application.Pokemon.Command;
using TCG.CatalogService.Application.Pokemon.Query;

namespace TCG.CatalogService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(string id)
    {
        try
        {
            var result = await _mediator.Send(new GetPokemonByIdQuery(id));
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(500);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] string setId, CancellationToken cancellationToken)
    {
        try
        {
            var pokemon = await _mediator.Send(new CreatePokemonCommand(setId));
            return Ok();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(500);
        }
        return Ok();
    }
}