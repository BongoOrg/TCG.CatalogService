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

public record GetItemsByNameQuery(string query, string[] idExtensions, int pageNumber, int pageSize) : MediatR.IRequest<List<ItemDto>>;

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

        var settings = new ConnectionSettings(new Uri("http://51.210.180.174:9200"))
        .DefaultIndex("items");

        var client = new ElasticClient(settings);

        var searchDescriptor = new SearchDescriptor<ElasticItem>()
     .AllIndices()
     .From(request.pageNumber * request.pageSize)
     .Size(request.pageSize)
     .Index("items")
     .Query(q => q //Query de base
         .Bool(b => b
             .Must(mu => mu
                 .MatchPhrasePrefix(m => m
                     .Field(f => f.Name)
                     .Query(request.query)
                 )
             )
         )
     );

        // vérifier si le premier élément n'est pas null
        if (request.idExtensions[0] != "undefined")
        {
            searchDescriptor.Query(q => q //Query si il y a des extensions
                .Bool(b => b
                    .Must(mu => mu
                        .MatchPhrasePrefix(m => m
                            .Field(f => f.Name)
                            .Query(request.query)
                        )
                    )
                    .Filter(fi => fi
                        .Terms(t => t
                            .Field(f => f.IdExtension)
                            .Terms(request.idExtensions)
                        )
                    )
                )
            );
        }

        var response = client.Search<ElasticItem>(searchDescriptor);


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

        return null;
            

    }
}