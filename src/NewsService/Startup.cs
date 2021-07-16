using HealthChecks.UI.Client;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.NewsService.Models.Dto.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.NewsService
{
    public class Startup : BaseApiInfo
    {
        public const string CorsPolicyName = "LtDoCorsPolicy";

        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly BaseServiceInfoConfig _serviceInfoConfig;

        public IConfiguration Configuration { get; }

        #region private methods

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username($"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}");
                        host.Password(_serviceInfoConfig.Id);
                    });
                });

                x.AddRequestClients(_rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<NewsServiceDbContext>();

            context.Database.Migrate();
        }

        #endregion

        #region public methods

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _rabbitMqConfig = Configuration
                .GetSection(BaseRabbitMqConfig.SectionName)
                .Get<RabbitMqConfig>();

            _serviceInfoConfig = Configuration
                .GetSection(BaseServiceInfoConfig.SectionName)
                .Get<BaseServiceInfoConfig>();

            Version = "1.2.4";
            Description = "NewsService, is intended to work with the news - create them, update info and etc.";
            StartTime = DateTime.UtcNow;
            ApiName = $"LT Digital Office - {_serviceInfoConfig.Name}";
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicyName,
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                "https://*.ltdo.xyz",
                                "http://*.ltdo.xyz",
                                "http://ltdo.xyz",
                                "http://ltdo.xyz:9808",
                                "http://localhost:4200",
                                "http://localhost:4500")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
            services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));
            services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));

            services.AddHttpContextAccessor();

            services.AddControllers().AddNewtonsoftJson().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            string connStr = Environment.GetEnvironmentVariable("ConnectionString");
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = Configuration.GetConnectionString("SQLConnectionString");
            }

            services.AddDbContext<NewsServiceDbContext>(options =>
            {
                options.UseSqlServer(connStr);
            });

            services.AddBusinessObjects();

            ConfigureMassTransit(services);

            services
                .AddHealthChecks()
                .AddSqlServer(connStr)
                .AddRabbitMqCheck();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            UpdateDatabase(app);

            app.UseForwardedHeaders();

            app.UseExceptionsHandler(loggerFactory);

            app.UseApiInformation();

            app.UseRouting();

            app.UseCors(CorsPolicyName);

            app.UseMiddleware<TokenMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(CorsPolicyName);

                endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
                {
                    ResultStatusCodes = new Dictionary<HealthStatus, int>
                    {
                        { HealthStatus.Unhealthy, 200 },
                        { HealthStatus.Healthy, 200 },
                        { HealthStatus.Degraded, 200 },
                    },
                    Predicate = check => check.Name != "masstransit-bus",
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

        #endregion
    }
}
