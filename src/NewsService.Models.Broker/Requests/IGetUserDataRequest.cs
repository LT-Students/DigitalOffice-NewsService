using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Configuration;
using System;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.GetUserDataEndpoint))]
    public interface IGetUserDataRequest
    {
        Guid UserId { get; }

        static object CreateObj(Guid userId)
        {
            return new
            {
                UserId = userId
            };
        }
    }
}
