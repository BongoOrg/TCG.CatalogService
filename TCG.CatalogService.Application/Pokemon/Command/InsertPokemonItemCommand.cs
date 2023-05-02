using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.Query;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Command
{
    public record InsertPokemonItemCommand(Item pokemon) : IRequest<Item>;

    public class InsertPokemonItemHandler : IRequestHandler<InsertPokemonItemCommand, Item>
    {
        private readonly ILogger<GetAllItemsQuery> _logger;
        private readonly IMongoRepository<Item> _mongoRepository;

        public InsertPokemonItemHandler(ILogger<GetAllItemsQuery> logger, IMongoRepository<Item> mongoRepository)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
        }

        public async Task<Item> Handle(InsertPokemonItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _mongoRepository.CreateAsync(request.pokemon);
                return request.pokemon;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while adding pokemon item");
                throw;
            }
        }
    }
}
