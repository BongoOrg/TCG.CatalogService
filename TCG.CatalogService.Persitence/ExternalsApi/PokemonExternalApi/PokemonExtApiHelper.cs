using System.Net;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Persitence.ExternalsApi.PokemonExternalApi;

public class PokemonExtApiHelper
{
    private static string DossierRacine { get; } = "D:/TCGPlace";
    
    private static string DossierPokemon { get; } = DossierRacine + "/Pokemon";

    public static void PrepareToDownload()
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

    public static void SetUpExtensionDirectory(string extensionId)
    {
        if (!Directory.Exists(DossierPokemon+ "/" + extensionId))
        {
            Directory.CreateDirectory(DossierPokemon + "/" + extensionId);
        }
    }

    public static void DownloadPokemonCard(Item pokemon)
    {
        SetUpExtensionDirectory(pokemon.IdExtension);
        if (!string.IsNullOrEmpty(pokemon.Image))
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(pokemon.Image + "/high.webp"), $"{DossierPokemon}/{pokemon.IdExtension}/{pokemon.IdCard}.webp");
            }
        }
    }
}
