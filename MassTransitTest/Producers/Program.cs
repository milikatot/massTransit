using MassTransit;
using Microsoft.Extensions.Options;
using Models;
using Serilog;
using Serilog.Events;

namespace Producers;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<MassTransitConfiguration>(hostContext.Configuration.GetSection(MassTransitConfiguration.MASS_TRANSIT_OPTION));
                services.AddSingleton(sp =>
                    sp.GetRequiredService<IOptions<MassTransitConfiguration>>().Value);

                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.AutoStart = true;

                        MassTransitConfiguration settings = context.GetRequiredService<MassTransitConfiguration>();

                        cfg.Host(new Uri(settings.RabbitMqHost), h =>
                        {
                            h.Username(settings.RabbitUser);
                            h.Password(settings.RabbitPass);
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });

                services.AddHostedService<Worker>();
            })
            .UseSerilog((host, log) =>
            {
                log.MinimumLevel.Debug();
                log.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                log.MinimumLevel.Override("Quartz", LogEventLevel.Information);
                log.WriteTo.Console();

            })
            .Build();

        host.Run();
    }
}