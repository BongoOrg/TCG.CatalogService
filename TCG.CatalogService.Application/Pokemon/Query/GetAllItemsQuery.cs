using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Query;

public record GetAllItemsQuery : IRequest<IEnumerable<Item>>;

public class GetAllItemsHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<Item>>
{
    private readonly IMongoRepository<Item> _repository;
    private readonly ILogger<GetAllItemsHandler> _logger;
    public GetAllItemsHandler(ILogger<GetAllItemsHandler> logger, IMongoRepository<Item> repository)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<IEnumerable<Item>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogWarning("Search post with id {SearchPostId} not found", request);
            var pokemon = await _repository.GetAllAsync();

            return new List<Item>(pokemon);
            
            /* List<PokemonDto> pokemonDto = new List<PokemonDto>();
             List<Domain.Pokemon> pokemonDomain = await _dbService.GetAsync();

             foreach(Domain.Pokemon pokemon in pokemonDomain)
             {
                 PokemonDto dto = new PokemonDto();
                 dto.Id = pokemon.Id;
                 dto.ItemName = pokemon.ItemName;
                 dto.Extension = pokemon.Extension;
                 dto.Language = pokemon.Language;
                 dto.Image = pokemon.Image;
                 pokemonDto.Add(dto);
             } */
        }
        catch (Exception e)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request, e.Message);
            throw;
        }
    }
}