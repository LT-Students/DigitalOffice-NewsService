using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Extensions;
using LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.Models.Broker.Models.TextTemplate;
using LT.DigitalOffice.Models.Broker.Requests.Admin;
using LT.DigitalOffice.Models.Broker.Requests.TextTemplate;
using LT.DigitalOffice.NewsService.Broker;
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
using Serilog;

namespace LT.DigitalOffice.NewsService
{
  public class Startup : BaseApiInfo
  {
    public const string CorsPolicyName = "LtDoCorsPolicy";

    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly BaseServiceInfoConfig _serviceInfoConfig;

    public IConfiguration Configuration { get; }

    #region private methods

    private (string username, string password) GetRabbitMqCredentials()
    {
      static string GetString(string envVar, string formAppsettings, string generated, string fieldName)
      {
        string str = Environment.GetEnvironmentVariable(envVar);
        if (string.IsNullOrEmpty(str))
        {
          str = formAppsettings ?? generated;

          Log.Information(
            formAppsettings == null
              ? $"Default RabbitMq {fieldName} was used."
              : $"RabbitMq {fieldName} from appsetings.json was used.");
        }
        else
        {
          Log.Information($"RabbitMq {fieldName} from environment was used.");
        }

        return str;
      }

      return (GetString("RabbitMqUsername", _rabbitMqConfig.Username, $"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}", "Username"),
        GetString("RabbitMqPassword", _rabbitMqConfig.Password, _serviceInfoConfig.Id, "Password"));
    }

    private void ConfigureMassTransit(IServiceCollection services)
    {
      (string username, string password) = GetRabbitMqCredentials();

      services.AddMassTransit(x =>
      {
        x.AddConsumer<GetNewsConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(_rabbitMqConfig.Host, "/", host =>
          {
            host.Username(username);
            host.Password(password);
          });

          cfg.ReceiveEndpoint(_rabbitMqConfig.GetNewsDataEndpoint, ep =>
          {
            ep.ConfigureConsumer<GetNewsConsumer>(context);
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

    private async void SendServiceEndpoints(IApplicationBuilder app)
    {
      IServiceProvider serviceProvider = app.ApplicationServices.GetRequiredService<IServiceProvider>();

      IRequestClient<ICreateServiceEndpointsRequest> rcCreateServiceEndpoints =
        serviceProvider.CreateRequestClient<ICreateServiceEndpointsRequest>(
          new Uri($"{_rabbitMqConfig.BaseUrl}/{_rabbitMqConfig.CreateServiceEndpointsEndpoint}"),
          default);

      Dictionary<string, Guid> endpointsIds =
        (await rcCreateServiceEndpoints.GetResponse<IOperationResult<Dictionary<string, Guid>>>(
          ICreateServiceEndpointsRequest.CreateObj(
            serviceName: _serviceInfoConfig.Name,
            endpointsNames: Enum.GetValues(typeof(ServiceEndpoints)).Cast<ServiceEndpoints>().Select(v => v.ToString()).ToList())))
        .Message.Body;

      Dictionary<int, List<string>> endpointsKeywords = KeywordCollector.GetEndpointKeywords();

      if (endpointsIds is not null && endpointsKeywords is not null)
      {
        IRequestClient<ICreateKeywordsRequest> rcCreateKeywords = serviceProvider.CreateRequestClient<ICreateKeywordsRequest>(
          new Uri($"{_rabbitMqConfig.BaseUrl}/{_rabbitMqConfig.CreateKeywordsEndpoint}"), default);

        List<EndpointKeywords> keywordsRequest = new();

        foreach (var endpointId in endpointsIds)
        {
          if (Enum.TryParse(endpointId.Key, out ServiceEndpoints serviceEndpoint))
          {
            keywordsRequest.Add(new EndpointKeywords(endpointId.Value, endpointsKeywords[(int)serviceEndpoint]));
          }
        }

        await rcCreateKeywords.GetResponse<IOperationResult<bool>>(
          ICreateKeywordsRequest.CreateObj(keywordsRequest));
      }
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

      Version = "1.2.5.2";
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
              .AllowAnyOrigin()
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

      SendServiceEndpoints(app);

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
