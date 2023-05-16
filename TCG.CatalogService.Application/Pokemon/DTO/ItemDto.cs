using Nest;

namespace TCG.CatalogService.Application.Pokemon.DTO;

public class ItemDto
{
    public string Name { get; set; }
    public string Image { get; set; }
    public string IdExtension { get; set; }
    public string Language { get; set; } = "fr";
    public string IdCard { get; set; }
}