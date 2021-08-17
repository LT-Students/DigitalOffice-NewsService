using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using LT.DigitalOffice.Models.Broker.Responses.User;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.Responses
{
    public class NewsResponseMapper : INewsResponseMapper
    {
        private IRequestClient<IGetUserDataRequest> _userRequestClient;
        private IRequestClient<IGetDepartmentRequest> _departmentRequestClient;
        private readonly ILogger _logger;

        private string GetUserFullName (Guid userId)
        {
            string fullName = null;

            try
            {
                var request = IGetUserDataRequest.CreateObj(userId);
                var response = _userRequestClient.GetResponse<IOperationResult<IGetUserDataResponse>>(request).Result;

                if (!response.Message.IsSuccess)
                {
                    _logger.LogWarning($"Can't found user FIO. Reason: '{string.Join(',', response.Message.Errors)}'");
                }
                else
                {
                    fullName = $"{response.Message.Body.LastName} {response.Message.Body.FirstName} {response.Message.Body.MiddleName}".Trim();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on get user FIO data request.");
            }

            return fullName;
        }

        private string GetDepartmentName(Guid departmentId)
        {
            string name = null;

            try
            {
                var request = IGetDepartmentRequest.CreateObj(null, departmentId);
                var response = _departmentRequestClient.GetResponse<IOperationResult<IGetDepartmentResponse>>(request).Result;

                if (!response.Message.IsSuccess)
                {
                    _logger.LogWarning($"Can't found department name. Reason: '{string.Join(',', response.Message.Errors)}'");
                }
                else
                {
                    name = response.Message.Body.Name;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on get department data request.");
            }

            return name;
        }

        public NewsResponseMapper(
            IRequestClient<IGetUserDataRequest> userClient,
            IRequestClient<IGetDepartmentRequest> departmentClient,
            ILogger<NewsResponseMapper> logger)
        {
            _userRequestClient = userClient;
            _departmentRequestClient = departmentClient;
            _logger = logger;
        }
        public NewsResponse Map(DbNews dbNews)
        {
            if (dbNews == null)
            {
                throw new ArgumentNullException(nameof(dbNews));
            }

            Department department = null;
            if(dbNews.DepartmentId != null && dbNews.DepartmentId != Guid.Empty)
            {
                department = new Department { Id = (Guid)dbNews.DepartmentId, Name = GetDepartmentName((Guid)dbNews.DepartmentId) };
            }

            return new NewsResponse
            {
                Id = dbNews.Id,
                Content = dbNews.Content,
                Subject = dbNews.Subject,
                Author = new User { Id = dbNews.AuthorId, FullName = GetUserFullName(dbNews.AuthorId) },
                Department = department,
                IsActive = dbNews.IsActive
            };
        }
    }
}
