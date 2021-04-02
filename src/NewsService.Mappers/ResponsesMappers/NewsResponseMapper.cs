using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
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
        private IRequestClient<IGetUserDataRequest> _requestClient;
        private readonly ILogger _logger;
        public NewsResponseMapper(
            [FromServices] IRequestClient<IGetUserDataRequest> client,
            ILogger<NewsResponseMapper> logger)
        {
            _requestClient = client;
            _logger = logger;
        }
        public NewsResponse Map(DbNews dbNews)
        {
            if (dbNews == null)
            {
                throw new BadRequestException();
            }

            User author = new User();
            User sender = new User();

            author.Id = dbNews.AuthorId;
            sender.Id = dbNews.SenderId;

            try
            {
                var authorRequest = IGetUserDataRequest.CreateObj(dbNews.AuthorId);
                var authorResponse = _requestClient.GetResponse<IOperationResult<IGetUserDataResponse>>(authorRequest).Result;

                if (!authorResponse.Message.IsSuccess)
                {
                    _logger.LogWarning($"Can not found author. Reason: '{string.Join(',', authorResponse.Message.Errors)}'");
                }

                author.FIO = $"{authorResponse.Message.Body.LastName} {authorResponse.Message.Body.FirstName} {authorResponse.Message.Body.MiddleName}".Trim();

                var senderRequest = IGetUserDataRequest.CreateObj(dbNews.SenderId);
                var senderResponse = _requestClient.GetResponse<IOperationResult<IGetUserDataResponse>>(senderRequest).Result;

                if (!senderResponse.Message.IsSuccess)
                {
                    _logger.LogWarning($"Can not found sender. Reason: '{string.Join(',', senderResponse.Message.Errors)}'");
                }

                sender.FIO = $"{senderResponse.Message.Body.LastName} {senderResponse.Message.Body.FirstName} {senderResponse.Message.Body.MiddleName}".Trim();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception on get user data request.");
            }

            return new NewsResponse
            {
                Id = dbNews.Id,
                Content = dbNews.Content,
                Subject = dbNews.Subject,
                Author = author,
                Sender = sender,
                CreatedAt = dbNews.CreatedAt
            };
        }
    }
}
