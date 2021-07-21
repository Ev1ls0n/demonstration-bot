using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using DemoBot.Services;

namespace DemoBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddJsonFile(path: "botconfig.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ConfigureWebhook>();
            services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(Configuration["BotToken"], httpClient));
            services.AddScoped<HandleUpdateService>();

            // The Telegram.Bot library heavily depends on Newtonsoft.Json library to deserialize
            // incoming webhook updates and send serialized responses back.
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                var token = Configuration["BotToken"];
                endpoints.MapControllerRoute(
                    name: "tgwebhook",
                    pattern: $"bot/{token}",
                    new { controller = "Webhook", action = "Post"});
                endpoints.MapControllers();
            });
        }
    }
}
