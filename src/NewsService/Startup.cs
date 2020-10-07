using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.NewsService.Business;
using LT.DigitalOffice.NewsService.Business.Interfaces;
using LT.DigitalOffice.NewsService.Data;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.NewsService.Mappers;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NewsService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMQOptions>(Configuration);

            services.AddHealthChecks();

            services.AddControllers();

            services.AddDbContext<NewsServiceDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLConnectionString"));
            });

            services.AddControllers();

            ConfigureCommands(services);
            ConfigureRepositories(services);
            ConfigureMappers(services);
            ConfigureMassTransit(services);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbitmqOptions = Configuration.GetSection(RabbitMQOptions.RabbitMQ).Get<RabbitMQOptions>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitmqOptions.Host, "/", host =>
                    {
                        host.Username($"{rabbitmqOptions.Username}_{rabbitmqOptions.Password}");
                        host.Password(rabbitmqOptions.Password);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/healthcheck");

            app.UseExceptionHandler(tempApp => tempApp.Run(CustomExceptionHandler.HandleCustomException));

            UpdateDatabase(app);

            app.UseHttpsRedirection();
            app.UseRouting();

            string corsUrl = Configuration.GetSection("Settings")["CorsUrl"];

            app.UseCors(builder =>
                builder
                    .WithOrigins(corsUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<NewsServiceDbContext>();
            context.Database.Migrate();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, NewsServiceDbContext>();

            services.AddTransient<INewsRepository, NewsRepository>();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<NewsRequest, DbNews>, NewsMapper>();
        }

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IEditNewsCommand, EditNewsCommand>();
        }
    }
}
