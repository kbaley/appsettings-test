using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConsole();
                })
                .UseConsoleLifetime()
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<HostedService>();
                })
                .Build();

            await host.RunAsync();

        }
    }

    class HostedService : IHostedService
    {
        private ILogger<HostedService> _logger;
        private IHostApplicationLifetime _appLifetime;
        private IConfiguration _config;

        public HostedService(ILogger<HostedService> logger, IHostApplicationLifetime appLifetime,
            IConfiguration configuration)
        {
            _logger = logger;   
            _appLifetime = appLifetime;
            _config = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hello world");
            if (_config != null) {
                var connString = _config.GetConnectionString("MyConnection");
                _logger.LogInformation($"Connection string: {connString == null}");
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
