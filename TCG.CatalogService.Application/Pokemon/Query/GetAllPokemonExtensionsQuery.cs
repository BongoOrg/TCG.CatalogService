
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.DTO;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Query
{

    public record GetAllPokemonExtensionsQuery() : IRequest<List<ExtensionDto>>;

    public class GetAllPokemonExtensionsQueryValidator : AbstractValidator<GetAllPokemonExtensionsQuery>
    {
        public GetAllPokemonExtensionsQueryValidator() { }
    }
    
    public class GetAllPokemonExtensionsQueryHandler : IRequestHandler<GetAllPokemonExtensionsQuery, List<ExtensionDto>>
    {
        private readonly ILogger<GetAllPokemonExtensionsQueryHandler> _logger;
        private readonly IMongoRepositoryExtension _repository;
        private readonly IMapper _mapper;

        public GetAllPokemonExtensionsQueryHandler(ILogger<GetAllPokemonExtensionsQueryHandler> logger, IMongoRepositoryExtension repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ExtensionDto>> Handle(GetAllPokemonExtensionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching pokemon extension ...");
            try {
                var pokemonExtensions = await _repository.GetAllAsync();
                var pokemonExtensionDtos = _mapper.Map<List<ExtensionDto>>(pokemonExtensions);
                return pokemonExtensionDtos;
            }
            catch (Exception e)
            {
                var errorMessage = $"Error in {nameof(GetAllPokemonExtensionsQueryHandler)}: {e.Message}";
                throw new Exception(errorMessage, e);
            }
        }
    }
}

