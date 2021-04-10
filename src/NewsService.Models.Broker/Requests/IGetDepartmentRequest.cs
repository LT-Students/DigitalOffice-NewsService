using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Configuration;
using System;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.GetDepartmentEndpoint))]
    public interface IGetDepartmentRequest
    {
        Guid DepartmentId { get; }

        static object CreateObj(Guid departmentId)
        {
            return new
            {
                DepartmentId = departmentId
            };
        }
    }
}