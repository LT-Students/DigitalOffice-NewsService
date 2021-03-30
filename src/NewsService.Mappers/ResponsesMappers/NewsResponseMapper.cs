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
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers
{
    public class NewsResponseMapper : INewsResponseMapper
    {
        private IRequestClient<IGetUserDataRequest> _client;
        public NewsResponseMapper(
            [FromServices] IRequestClient<IGetUserDataRequest> client)
        {
            _client = client;
        }
        public async Task<NewsResponse> Map(DbNews value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            var authorRequest = IGetUserDataRequest.CreateObj(value.AuthorId);

            var authorResponse = await _client.GetResponse<IOperationResult<IGetUserDataResponse>>(authorRequest);

            var senderRequest = IGetUserDataRequest.CreateObj(value.SenderId);

            var senderResponse = await _client.GetResponse<IOperationResult<IGetUserDataResponse>>(senderRequest);

            return new NewsResponse
            {
                Id = value.Id,
                Content = value.Content,
                Subject = value.Subject,
                Author = new User
                {
                    Id = authorResponse.Message.Body.Id,
                    FIO = $"{authorResponse.Message.Body.LastName} {authorResponse.Message.Body.FirstName} {authorResponse.Message.Body.MiddleName}".Trim()
                },
                Sender = new User
                {
                    Id = senderResponse.Message.Body.Id,
                    FIO = $"{senderResponse.Message.Body.LastName} {senderResponse.Message.Body.FirstName} {senderResponse.Message.Body.MiddleName}".Trim()
                },
                CreatedAt = value.CreatedAt
            };
        }
    }
}
