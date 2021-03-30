using System;

namespace LT.DigitalOffice.NewsService.Models.Broker.Responses
{
    public interface IGetFIOUserResponse
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool IsActive { get; set; }

        static object CreateObj(Guid id, string firstName, string lastName, bool isActive)
        {
            return new
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                IsActive = isActive
            };
        }
    }
}
