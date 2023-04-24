using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TCG.CatalogService.API.Response;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.Command;
using TCG.CatalogService.Application.Pokemon.Query;

namespace TCG.CatalogService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPokemonExternalRepository _externalRepository;
        private readonly IMapper _mapper;
        
        public PokemonController(IMediator mediator, IPokemonExternalRepository pokemonExternalRepository, IMapper mapper)
        {
            _mediator = mediator;
            _externalRepository = pokemonExternalRepository;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("ImportAllPokemonsCardsFromAllPokemonsSets")]
        public async Task<IActionResult> ImportAllPokemonsCardsFromAllPokemonsSets()
        {
            {
                try
                {
                    var listePokemonsByExt = await _externalRepository.GetAllPokemonCardsBySets();
                    foreach (var pokemonsByExt in listePokemonsByExt)
                    {
                        foreach (var pokemon in pokemonsByExt)
                        {
                            await _mediator.Send(new InsertPokemonItemCommand(pokemon));
                        }
                    }
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
            }
        }

        [HttpPost]
        [Route("DownloadAllPokemonsCardsImagesFromAllPokemonsSets")]
        public async Task<IActionResult> DownloadAllPokemonsCardsImagesFromDatabase()
        {
            {
                try
                {
                    var result = await _mediator.Send(new GetAllItemsQuery());
                    var pokemons = _mapper.Map<List<PokemonResponse>>(result);
                    return Ok(pokemons);
                    //var pokemons = _mapper.Map<List<PokemonMongoDB>>(result);
                    //PokemonExtApiHelper.PrepareToDownload();
                    //foreach (var pokemon in pokemons)
                    //{
                    //    PokemonExtApiHelper.DownloadPokemonCard(pokemon);
                    //}

                    //return Ok();
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
        }
    }
}
