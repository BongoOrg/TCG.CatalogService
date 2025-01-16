using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.IHelpers;

public interface IPokemonExtApiHelper
{
    public string DownloadPokemonCard(Item pokemon);
    public string DownloadExtensions(Extension extension);
    public void SetUpExtensionDirectory(string extensionId);
    public void PrepareToDownload();

}