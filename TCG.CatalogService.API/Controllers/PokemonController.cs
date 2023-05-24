using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TCG.CatalogService.API.Response;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.Command;
using TCG.CatalogService.Application.Pokemon.Query;
using TCG.CatalogService.Domain;
using TCG.CatalogService.Persitence.ExternalsApi.PokemonExternalApi;

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

        [HttpPost]
        [Route("DownloadAllPokemonsCardsImagesFromDatabase")]
        public async Task<IActionResult> DownloadAllPokemonsCardsImagesFromDatabase()
        {
            var result = await _mediator.Send(new GetAllItemsQuery());
            var pokemons = _mapper.Map<List<Item>>(result);
            
            PokemonExtApiHelper.PrepareToDownload();

            foreach (var pokemon in pokemons)
            {
                PokemonExtApiHelper.DownloadPokemonCard(pokemon);
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetAllPokemonsExtensions")]
        public async Task<IActionResult> GetAllPokemonsExtensions()
        {
            var result =  await _mediator.Send(new GetAllPokemonExtensionsQuery());

            return Ok(result);
        }
        
        [HttpPost]
        [Route("InsertAllPokemonsExtensions")]
        public async Task<IActionResult> InsertAllPokemonsExtensions()
        {
            var result = await _mediator.Send(new InsertPokemonExtensionsCommand());
            return Ok(result);
        }
    }
}
