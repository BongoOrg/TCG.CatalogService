namespace TCG.CatalogService.API.Response
{
    public class PokemonResponse
    {
        public string IdExtension { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Language { get; set; } = "fr";
        public string IdCard { get; set; }
    }
}
