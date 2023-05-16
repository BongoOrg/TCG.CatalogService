using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.DTO;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Query;

public record GetPokemonByIdQuery(string id) : IRequest<ItemDto>;

public class GetItemByIdValidator : AbstractValidator<GetPokemonByIdQuery>
{
    public GetItemByIdValidator() { }
}

public class GetPokemonByIdQueryHandler : IRequestHandler<GetPokemonByIdQuery, ItemDto>
{
    private readonly ILogger<GetPokemonByIdQueryHandler> _logger;
    private readonly IMongoRepositoryItem _repository;
    private readonly IMapper _mapper;

    public GetPokemonByIdQueryHandler(ILogger<GetPokemonByIdQueryHandler> logger, IMongoRepositoryItem repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ItemDto> Handle(GetPokemonByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pokemon = await _repository.GetAsync(request.id);

            if (pokemon == null)
            {
                _logger.LogWarning("Search post with id {PokemonIdCard} not found", request.id);
                return null;
            }

            var pokemonDto = _mapper.Map<ItemDto>(pokemon);

            return pokemonDto;
        }
        catch (Exception e)
        {
            _logger.LogError("Error retrieving search post with id {PokemonCardId}: {ErrorMessage}", request.id, e.Message);
            throw;
        }
    }
}