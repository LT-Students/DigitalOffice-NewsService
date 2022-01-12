using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Models.Broker.Requests.Admin;
using LT.DigitalOffice.Models.Broker.Requests.Department;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.News;
using LT.DigitalOffice.Models.Broker.Requests.TextTemplate;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.NewsService.Models.Dto.Configuration
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    [AutoInjectRequest(typeof(ICreateServiceEndpointsRequest))]
    public string CreateServiceEndpointsEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICreateKeywordsRequest))]
    public string CreateKeywordsEndpoint { get; set; }

    public string GetNewsDataEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetImagesRequest))]
    public string GetImagesEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetUsersDataRequest))]
    public string GetUsersDataEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetDepartmentsRequest))]
    public string GetDepartmentsEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICreateDepartmentEntityRequest))]
    public string CreateDepartmentEntityEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICheckUsersExistence))]
    public string CheckUsersExistenceEndpoint { get; set; }
  }
}
