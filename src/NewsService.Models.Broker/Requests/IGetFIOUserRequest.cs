using System;

namespace LT.DigitalOffice.NewsService.Models.Broker.Requests
{
    public interface IGetFIOUserRequest
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
