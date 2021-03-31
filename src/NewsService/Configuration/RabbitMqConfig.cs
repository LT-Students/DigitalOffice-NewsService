using LT.DigitalOffice.Kernel.Broker;

namespace LT.DigitalOffice.NewsService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqOptions
    {
        public string GetUserDataEndpoint { get; set; }
    }
}
