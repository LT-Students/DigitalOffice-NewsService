using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Broker.Requests;
using LT.DigitalOffice.NewsService.Models.Broker.Responses;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Model;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers
{
    public class NewsResponseMapper : INewsResponseMapper
    {
        private IRequestClient<IGetFIOUserRequest> _client;

        public NewsResponseMapper(
            [FromServices] IRequestClient<IGetFIOUserRequest> client)
        {
            _client = client;
        }

        public NewsResponse Map(DbNews value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            var authorResponse =  _client.GetResponse<IGetFIOUserResponse>(
                IGetFIOUserRequest.CreateObj(value.AuthorId)).Result;

            var senderResponse = _client.GetResponse<IGetFIOUserResponse>(
                IGetFIOUserRequest.CreateObj(value.SenderId)).Result;

            return new NewsResponse
            {
                Id = value.Id,
                Content = value.Content,
                Subject = value.Subject,
                Author = new User
                {
                    Id = authorResponse.Message.Id,
                    FIO = value.Pseudonym
                },
                Sender = new User
                {
                    Id = senderResponse.Message.Id,
                    FIO = $"{senderResponse.Message.LastName} {senderResponse.Message.FirstName}"
                },
                CreatedAt = value.CreatedAt
            };
        }
    }
}