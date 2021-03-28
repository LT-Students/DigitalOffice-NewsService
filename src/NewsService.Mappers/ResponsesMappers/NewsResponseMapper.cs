using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
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
        private IRequestClient<IGetUserInfoRequest> _client;
        public NewsResponseMapper(
            [FromServices] IRequestClient<IGetUserInfoRequest> client)
        {
            _client = client;
        }
        public async Task<NewsResponse> Map(DbNews value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            var authorRequest = IGetUserInfoRequest.CreateObj(value.AuthorId);

            var authorResponse = await _client.GetResponse<IGetUserInfoResponse>(authorRequest);

            var senderResponse = _client.GetResponse<IGetUserInfoResponse>(IGetUserInfoRequest.CreateObj(value.SenderId));

            return new NewsResponse
            {
                Id = value.Id,
                Content = value.Content,
                Subject = value.Subject,
                Author = new User
                {
                    Id = authorResponse.Message.Id,
                    FIO = $"{authorResponse.Message.LastName} {authorResponse.Message.FirstName}"// {authorResponse.Message.}"
                },
                Sender = new User
                {
                    Id = senderResponse.Result.Message.Id,
                    FIO = $"{senderResponse.Result.Message.LastName} {senderResponse.Result.Message.FirstName}"// {authorResponse.Message.}"
                },
                CreatedAt = value.CreatedAt
            };
        }
    }
}
