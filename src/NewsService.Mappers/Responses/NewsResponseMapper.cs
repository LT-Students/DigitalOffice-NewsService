using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
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

        private string GetUserFIO (Guid userId)
        {
            string fio = null;

            try
            {
                var request = IGetUserDataRequest.CreateObj(userId);
                var response = _userRequestClient.GetResponse<IOperationResult<IGetUserDataResponse>>(request).Result;

                if (!response.Message.IsSuccess)
                {
                    _logger.LogWarning($"Can't found user FIO. Reason: '{string.Join(',', response.Message.Errors)}'");
                }

                fio = $"{response.Message.Body.LastName} {response.Message.Body.FirstName} {response.Message.Body.MiddleName}".Trim();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on get user FIO data request.");
            }

            return fio;
        }

        private string GetDepartmentName(Guid departmentId)
        {
            string name = null;

            try
            {
                var request = IGetDepartmentRequest.CreateObj(departmentId);
                var response = _departmentRequestClient.GetResponse<IOperationResult<IGetDepartmentResponse>>(request).Result;

                if (!response.Message.IsSuccess)
                {
                    _logger.LogWarning($"Can't found department name. Reason: '{string.Join(',', response.Message.Errors)}'");
                }

                name = response.Message.Body.Name;
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
                throw new BadRequestException();
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
                Author = new User { Id = dbNews.AuthorId, FIO = GetUserFIO(dbNews.AuthorId) },
                Sender = new User { Id = dbNews.AuthorId, FIO = GetUserFIO(dbNews.AuthorId) },
                CreatedAt = dbNews.CreatedAt,
                Department = department,
                IsActive = dbNews.IsActive
            };
        }
    }
}
