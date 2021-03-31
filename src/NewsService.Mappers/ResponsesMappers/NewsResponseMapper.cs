using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
        public async Task<NewsResponse> Map(DbNews value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            User author = new User();
            User sender = new User();

            try
            {
                var authorRequest = IGetUserDataRequest.CreateObj(value.AuthorId);
                var authorResponse = await _client.GetResponse<IOperationResult<IGetUserDataResponse>>(authorRequest);
                author.Id = authorResponse.Message.Body.Id;
                author.FIO = $"{authorResponse.Message.Body.LastName} {authorResponse.Message.Body.FirstName} {authorResponse.Message.Body.MiddleName}".Trim();

                var senderRequest = IGetUserDataRequest.CreateObj(value.SenderId);
                var senderResponse = await _client.GetResponse<IOperationResult<IGetUserDataResponse>>(senderRequest);
                sender.Id = senderResponse.Message.Body.Id;
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
