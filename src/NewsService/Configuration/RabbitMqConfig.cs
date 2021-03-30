using LT.DigitalOffice.Kernel.Broker;

namespace LT.DigitalOffice.NewsService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqOptions
    {
        public string GetUserInfoEndpoint { get; set; }
    }
}