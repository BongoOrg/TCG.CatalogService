using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Command;

public record CreatePokemonCommand(string setId) : IRequest<IEnumerable<Item>>;
public class InsertPokemonItemValidator: AbstractValidator<CreatePokemonCommand> 
{
    public InsertPokemonItemValidator() { }
}
public class CreatePokemonCommandHandler : IRequestHandler<CreatePokemonCommand, IEnumerable<Item>>
{

    private readonly ILogger _logger;
    private readonly IMongoRepository<Item> _repository;
    private readonly IPokemonExternalRepository _externalRepository;

    public CreatePokemonCommandHandler(ILogger<CreatePokemonCommandHandler> logger, IMongoRepository<Item> repository, IPokemonExternalRepository pokemonExternalRepository)
    {
        _logger = logger;
        _repository = repository;
        _externalRepository = pokemonExternalRepository;
    }
    
    public async Task<IEnumerable<Item>> Handle(CreatePokemonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<Item> items = new List<Item>();
            var pokemons = await _externalRepository.GetPokemonCardsExtensionList(request.setId);
            await _repository.CreateManyAsync(pokemons);
            return items.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error while adding Pokemon");
            throw;
        }
    }
}
