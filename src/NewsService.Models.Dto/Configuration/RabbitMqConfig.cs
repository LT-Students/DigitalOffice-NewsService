using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.NewsService.Models.Dto.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        [AutoInjectRequest(typeof(IGetUserDataRequest))]
        public string GetUserDataEndpoint { get; set; }

        [AutoInjectRequest(typeof(IGetDepartmentRequest))]
        public string GetDepartmentEndpoint { get; set; }
    }
}