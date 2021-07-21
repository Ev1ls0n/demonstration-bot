using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace DemoBot.Services
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        public ConfigureWebhook(ILogger<ConfigureWebhook> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _services = serviceProvider;
            
            var builder = new ConfigurationBuilder().AddJsonFile(path: "botconfig.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var webhookAddress = @$"{_configuration["HostAddress"]}/bot/{_configuration["BotToken"]}";

            _logger.LogInformation($"(i) Setting webhook: {webhookAddress}");

            await botClient.SetWebhookAsync(webhookAddress);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Удаление Webhook'а после закрытия приложения
            _logger.LogInformation("(i) Removing webhook");
            await botClient.DeleteWebhookAsync();
        }
    }
}
