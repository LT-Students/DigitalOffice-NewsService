using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Broker.Requests;
using LT.DigitalOffice.NewsService.Models.Broker.Responses;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Model;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers
{
    public class NewsResponseMapper : INewsResponseMapper
    {
        private IRequestClient<IGetUserDataRequest> _client;
        private readonly ILogger _logger;
        public NewsResponseMapper(
            [FromServices] IRequestClient<IGetUserDataRequest> client,
            ILogger<NewsResponseMapper> logger)
        {
            _client = client;
            _logger = logger;
        }
        public NewsResponse Map(DbNews value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            User author = new User { Id = value.Id };
            User sender = new User { Id = value.Id };

            try
            {
                var authorRequest = IGetUserDataRequest.CreateObj(value.AuthorId);
                var authorResponse = _client.GetResponse<IOperationResult<IGetUserDataResponse>>(authorRequest).Result;
                author.FIO = $"{authorResponse.Message.Body.LastName} {authorResponse.Message.Body.FirstName} {authorResponse.Message.Body.MiddleName}".Trim();

                var senderRequest = IGetUserDataRequest.CreateObj(value.SenderId);
                var senderResponse = _client.GetResponse<IOperationResult<IGetUserDataResponse>>(senderRequest).Result;
                sender.FIO = $"{senderResponse.Message.Body.LastName} {senderResponse.Message.Body.FirstName} {senderResponse.Message.Body.MiddleName}".Trim();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on get user data request.");
            }

            return new NewsResponse
            {
                Id = value.Id,
                Content = value.Content,
                Subject = value.Subject,
                Author = author,
                Sender = sender,
                CreatedAt = value.CreatedAt
            };
        }

    }
}