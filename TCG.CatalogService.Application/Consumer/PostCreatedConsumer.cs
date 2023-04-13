using MassTransit;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;
using TCG.Common.MassTransit.Messages;

namespace TCG.CatalogService.Application.Consumer;

public class PostCreatedConsumer : IConsumer<PostCreated>
{
    private readonly IMongoRepository<Item> _repository;

    public PostCreatedConsumer(IMongoRepository<Item> repository)
    {
        _repository = repository;
    }
        
    public async Task Consume(ConsumeContext<PostCreated> context)
    {
        var message = context.Message;

        var item = await _repository.GetAsync(message.itemId);
        var response = new PostCreatedResponse(item.Name, item.Image);
        await context.RespondAsync(response);
    }
}