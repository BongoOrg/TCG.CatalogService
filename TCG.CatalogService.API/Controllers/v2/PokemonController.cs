﻿using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.Command;
using TCG.CatalogService.Application.Pokemon.Query;
using TCG.CatalogService.Domain;
using TCG.CatalogService.Persistence.ExternalsApi.PokemonExternalApi;

namespace TCG.CatalogService.API.Controllers.v2
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("2.0")]
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
            await _mediator.Send(new InsertAllPokemonsCommand());
            return Ok();
        }

        // [HttpPost]
        // [Route("DownloadAllPokemonsCardsImagesFromDatabase")]
        // public async Task<IActionResult> DownloadAllPokemonsCardsImagesFromDatabase()
        // {
        //     var result = await _mediator.Send(new GetAllItemsQuery());
        //     var pokemons = _mapper.Map<List<Item>>(result);
        //
        //     PokemonExtApiHelper.PrepareToDownload();
        //
        //     foreach (var pokemon in pokemons)
        //     {
        //         PokemonExtApiHelper.DownloadPokemonCard(pokemon);
        //     }
        //
        //     return Ok();
        // }

        [HttpGet]
        [Route("GetAllPokemonsExtensions")]
        public async Task<IActionResult> GetAllPokemonsExtensions()
        {
            var result = await _mediator.Send(new GetAllPokemonExtensionsQuery());

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
