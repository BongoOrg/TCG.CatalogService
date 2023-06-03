using MassTransit;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;
using TCG.Common.MassTransit.Messages;

namespace TCG.CatalogService.Application.Consumer;

public class PostCreatedConsumer : IConsumer<PostCreated>
{
    private readonly IMongoRepositoryItem _repository;

    public PostCreatedConsumer(IMongoRepositoryItem repository)
    {
        _repository = repository;
    }
        
    public async Task Consume(ConsumeContext<PostCreated> context)
    {
        var message = context.Message;

        var item = await _repository.GetAsync(message.itemId);
        var response = new PostCreatedResponse(item.Name, item.Image, item.IdExtension, item.LibelleExtension);
        await context.RespondAsync(response);
    }
}