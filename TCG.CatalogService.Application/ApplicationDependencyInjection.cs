using System.Reflection;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCG.CatalogService.Application.Consumer;
using TCG.Common.Settings;

namespace TCG.CatalogService.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services.AddMediatR(assembly);
    }
    
    public static IServiceCollection AddMassTransitWithRabbitMQQQQ(this IServiceCollection serviceCollection)
    {
        //Config masstransit to rabbitmq
        serviceCollection.AddMassTransit(configure =>
        {
            configure.AddConsumer<PostCreatedConsumer>();
            configure.UsingRabbitMq((context, configurator) =>
            {
                var config = context.GetService<IConfiguration>();
                //On récupère le nom de la table Catalog
                ////On recupère la config de seeting json pour rabbitMQ
                var rabbitMQSettings = config.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                configurator.Host(new Uri(rabbitMQSettings.Host));
                configurator.ConfigureEndpoints(context);
                // Retry policy for consuming messages
                configurator.UseMessageRetry(retryConfig =>
                {
                    // Exponential back-off (second argument is the max retry count)
                    retryConfig.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(3));
                });
                
                //Defnir comment les queues sont crées dans rabbit
                configurator.ReceiveEndpoint("catalogservice", e =>
                {
                    e.UseMessageRetry(r => r.Interval(2, 3000));
                    e.ConfigureConsumer<PostCreatedConsumer>(context);
                });
            });
        });
        //Start rabbitmq bus pour exanges
        serviceCollection.AddMassTransitHostedService();
        return serviceCollection;
    }
}