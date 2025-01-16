using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.IHelpers;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Pokemon.Command
{
    public record InsertPokemonExtensionsCommand() : IRequest;

    public class InsertPokemonExtensionsHandler : IRequestHandler<InsertPokemonExtensionsCommand, Unit>
    {
        private readonly ILogger<InsertPokemonExtensionsCommand> _logger;
        private readonly IMongoRepositoryExtension _mongoRepository;
        private readonly IPokemonExternalRepository _pokemonExternalRepository;
        private readonly IPictureHelper _pictureHelper;
        private readonly IPokemonExtApiHelper _pokemonExtApiHelper;
        private string blobStorageContainerName = "pokemon-extensions";
        private string AWSStorageContainerName = "tcg-bucket-images";

        public InsertPokemonExtensionsHandler(ILogger<InsertPokemonExtensionsCommand> logger, IPokemonExtApiHelper pokemonExtApiHelper,IMongoRepositoryExtension mongoRepository, IPokemonExternalRepository pokemonExternalRepository, IPictureHelper pictureHelper)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
            _pokemonExternalRepository = pokemonExternalRepository;
            _pictureHelper = pictureHelper;
            _pokemonExtApiHelper = pokemonExtApiHelper;
        }

        public async Task<Unit> Handle(InsertPokemonExtensionsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting pokemon extension insertion process ...");
            _pokemonExtApiHelper.PrepareToDownload();
            try
            { 
                _logger.LogInformation("Fetching pokemon extension ...");
                var pokemonExtensions = await _pokemonExternalRepository.GetPokemonExtensionList();
                foreach (var item in pokemonExtensions)
                {
                    item.Symbole = (item.Symbole != null) ? item.Symbole : item.Symbole;
                    item.Logo = (item.Logo != null) ? item.Logo : item.Symbole;
                    
                    if (item.Symbole.IsNullOrEmpty())
                    {
                        item.Symbole = null;
                        item.Logo = null;
                    }
                    else
                    {
                        //A changer pour DL sur un bucket
                        //item.Symbole = await _pictureHelper.SavePictureToAWSS3(item.Id, _pictureHelper.GetBytes(item.Symbole), AWSStorageContainerName);
                        item.Logo = _pokemonExtApiHelper.DownloadExtensions(item);
                    }
                    await _mongoRepository.CreateAsync(item);
                };
                _logger.LogInformation("Pokemon extension inserted.");
                return Unit.Value;
            }
            catch (Exception e)
            {
                var errorMessage = $"Error while adding pokemon extension in {nameof(InsertPokemonExtensionsHandler)}: {e.Message}";
                throw new Exception(errorMessage, e);
            }
        }
    }
}
