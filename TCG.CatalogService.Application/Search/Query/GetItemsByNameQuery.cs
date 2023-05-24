using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Application.Pokemon.DTO;
using TCG.CatalogService.Domain;
using Elasticsearch.Net;
using Nest;
using TCG.CatalogService.Domain.ElasticSearchDto;

namespace TCG.CatalogService.Application.Pokemon.Query;

public record GetItemsByNameQuery(string query) : MediatR.IRequest<List<ItemDto>>;

public class GetItemsByNameValidator : AbstractValidator<GetItemsByNameQuery>
{
    public GetItemsByNameValidator() { }
}

public class GetItemsByNameQueryHandler : IRequestHandler<GetItemsByNameQuery, List<ItemDto>>
{
    private readonly ILogger<GetItemsByNameQueryHandler> _logger;
    private readonly IMongoRepository<Item> _repository;
    private readonly IMapper _mapper;

    public GetItemsByNameQueryHandler(ILogger<GetItemsByNameQueryHandler> logger, IMongoRepository<Item> repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<ItemDto>> Handle(GetItemsByNameQuery request, CancellationToken cancellationToken)
    {

        /* var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
         //var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
         .ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
         .CertificateFingerprint("D2:72:A0:65:91:B5:6E:D6:2C:36:05:E1:7A:38:4A:4C:6C:DC:E1:F5")
         .Authentication(new BasicAuthentication("elastic", "ILoveCarapuce"));
         var client = new ElasticsearchClient(settings); */

        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("items");

        var client = new ElasticClient(settings);

        var response = client.Search<ElasticItem>(s => s
            .AllIndices()
            .From(0)
            .Size(10)
            .Index("items")
            .Query(q => q
                .MatchPhrasePrefix(m => m
                    .Field(f => f.Name)
                    .Query(request.query)
                )
        )
        );

        if (response.IsValid)
        {
            var g = response.Documents;
            foreach (var book in response.Documents)
            {
                //Console.WriteLine($"Name: {book.hits.hits._source.Name}");
            }

            var item = response.Documents;
            return _mapper.Map<List<ItemDto>>(item); ;
        }

        if (!response.IsValid)
        {
            Console.WriteLine("Erreur : " + response.DebugInformation);
            if (response.ServerError != null)
            {
                Console.WriteLine("Erreur serveur : " + response.ServerError.Error);
            }
        }

        /* try
         {
             var response = await client.SearchAsync<Item>(s => s
                 .Index("items")
                 .From(0)
                 .Size(10)
                 .Query(q => q
                     .Term(t => t.Name, request.query)
                 )
             );

             if (response.IsValidResponse)
             {
                 var item = response.Documents.FirstOrDefault();
                 return _mapper.Map<ItemDto>(item); ;
             }

             if (!response.IsValidResponse)
             {
                 Console.WriteLine("Erreur : " + response.DebugInformation);
                 if (response.ElasticsearchServerError != null)
                 {
                     Console.WriteLine("Erreur serveur : " + response.ElasticsearchServerError.Error);
                 }
             } 

                 }
         catch (Exception e)
         {
             _logger.LogError("Error retrieving search post with id {PokemonCardId}: {ErrorMessage}", request.query, e.Message);
             throw;
         }
        */

        return null;
            

    }
}