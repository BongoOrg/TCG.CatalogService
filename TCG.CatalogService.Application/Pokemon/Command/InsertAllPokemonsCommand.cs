using FluentValidation;
using MassTransit.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.IHelpers;
using TCG.CatalogService.Application.Pokemon.Query;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Command
{
    public record InsertAllPokemonsCommand() : IRequest;

    public class InsertAllPokemonsHandler : IRequestHandler<InsertAllPokemonsCommand>
    {
        private readonly ILogger<GetAllItemsQuery> _logger;
        private readonly IMongoRepository<Item> _mongoRepository;
        private readonly IPokemonExternalRepository _externalRepository;
        private readonly IPictureHelper _pictureHelper;
        private readonly IPokemonExtApiHelper _pokemonExtApiHelper;
        private string blobStorageContainerName = "pokemon-references";
        private string OvhStorageContainerName = "tcgplaceblob";
        private string AWSStorageContainerName = "tcg-bucket-images";

        public InsertAllPokemonsHandler(ILogger<GetAllItemsQuery> logger, IPokemonExtApiHelper pokemonExtApiHelper,IMongoRepository<Item> mongoRepository, IPokemonExternalRepository pokemonExternalRepository, IPictureHelper pictureHelper)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
            _externalRepository = pokemonExternalRepository;
            _pictureHelper = pictureHelper;
            _pokemonExtApiHelper = pokemonExtApiHelper;
        }

        public async Task<Unit> Handle(InsertAllPokemonsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var listePokemonsByExt = await _externalRepository.GetAllPokemonCardsBySets();
                _pokemonExtApiHelper.PrepareToDownload();
                 foreach (var pokemonsByExt in listePokemonsByExt)
                {
                    foreach (var pokemon in pokemonsByExt)
                    {
                        if (pokemon.Image == null || pokemon.Image == string.Empty)
                        {
                            pokemon.Image = null;
                        }
                        else
                        {
                            //A changer pour DL sur un Bucket
                            //pokemon.Image =  await _pictureHelper.SavePictureToAWSS3(pokemon.IdCard, _pictureHelper.GetBytes(pokemon.Image + "/high.webp"), AWSStorageContainerName);
                            pokemon.Image = _pokemonExtApiHelper.DownloadPokemonCard(pokemon);
                        }                        
                        await _mongoRepository.CreateAsync(pokemon);
                    }
                }
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
