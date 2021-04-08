using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.NewsService.Models.Dto.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetUserDataEndpoint { get; set; }
    }
}