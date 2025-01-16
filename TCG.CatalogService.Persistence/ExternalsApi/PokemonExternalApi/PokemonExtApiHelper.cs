using System.Net;
using TCG.CatalogService.Application.IHelpers;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Persistence.ExternalsApi.PokemonExternalApi;

public class PokemonExtApiHelper : IPokemonExtApiHelper 
{
    private static string DossierRacine { get; } = "/Users/val/Documents/Pokemons";
    
    private static string DossierPokemon { get; } = DossierRacine + "/PokemonsCards";
    private static string DossierPokemonExtensions { get; } = DossierRacine + "/Extensions";

    public PokemonExtApiHelper()
    {
        
    }

    public void PrepareToDownload()
    {
        if (!Directory.Exists(DossierRacine))
        {
            // Créer le dossier racine
            Directory.CreateDirectory(DossierRacine);
        }
        
        // Vérifie si le dossier existe
        if (Directory.Exists(DossierPokemon))
        {
            // Supprime le dossier pokemon
            Directory.Delete(DossierPokemon, true);
        }

        Directory.CreateDirectory(DossierPokemon);
        
    }

    public void SetUpExtensionDirectory(string extensionId)
    {
        if (!Directory.Exists(DossierPokemon+ "/" + extensionId))
        {
            Directory.CreateDirectory(DossierPokemon + "/" + extensionId);
        }
    }

    public string DownloadPokemonCard(Item pokemon)
    {
        SetUpExtensionDirectory(pokemon.IdExtension);
        if (!string.IsNullOrEmpty(pokemon.Image))
        {
            using (WebClient client = new WebClient())
            {
                var fileName = $"{DossierPokemon}/{pokemon.IdExtension}/{pokemon.IdCard}.webp";
                var test = $"http://127.0.0.1:8088/PokemonsCards/{pokemon.IdExtension}/{pokemon.IdCard}.webp";
                client.DownloadFile(new Uri(pokemon.Image + "/high.webp"), fileName);
                return test;
            }
        }
        else
        {
            return "";
        }
    }
    
    public string DownloadExtensions(Extension extension)
    {
        if (!string.IsNullOrEmpty(extension.Id))
        {
            using (WebClient client = new WebClient())
            {
                var fileName = $"{DossierPokemonExtensions}/{extension.Id}.webp";
                var test = $"http://127.0.0.1:8088/Extensions/{extension.Id}.webp";
                client.DownloadFile(new Uri(extension.Logo + ".webp"), fileName);
                return test;
            }
        }
        else
        {
            return "";
        }
    }
}
