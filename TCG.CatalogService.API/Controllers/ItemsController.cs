using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.CatalogService.API.Response;
using TCG.CatalogService.Application.Pokemon.Command;
using TCG.CatalogService.Application.Pokemon.Query;

namespace TCG.CatalogService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ItemsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PokemonResponse>>> Get()
    {
        var result = await _mediator.Send(new GetAllItemsQuery());
        var pokemons = _mapper.Map<List<PokemonResponse>>(result);
        return Ok(pokemons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(string id)
    {
        var result = await _mediator.Send(new GetPokemonByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] string setId, CancellationToken cancellationToken)
    {
        var pokemon = await _mediator.Send(new CreatePokemonCommand(setId));
        return Ok();
    }
}