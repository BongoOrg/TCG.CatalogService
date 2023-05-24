using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Command
{
    public record InsertPokemonExtensionsCommand() : IRequest;

    public class InsertPokemonExtensionsHandler : IRequestHandler<InsertPokemonExtensionsCommand, Unit>
    {
        private readonly ILogger<InsertPokemonExtensionsCommand> _logger;
        private readonly IMongoRepositoryExtension _mongoRepository;
        private readonly IPokemonExternalRepository _pokemonExternalRepository;

        public InsertPokemonExtensionsHandler(ILogger<InsertPokemonExtensionsCommand> logger, IMongoRepositoryExtension mongoRepository, IPokemonExternalRepository pokemonExternalRepository)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
            _pokemonExternalRepository = pokemonExternalRepository;
        }

        public async Task<Unit> Handle(InsertPokemonExtensionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pokemonExtensions = await _pokemonExternalRepository.GetPokemonExtensionList();
                await _mongoRepository.CreateManyAsync((IEnumerable<Extension>)pokemonExtensions);
                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while adding pokemon item");
                throw;
            }
        }
    }
}
