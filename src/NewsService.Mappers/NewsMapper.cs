using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using System;

namespace LT.DigitalOffice.NewsService.Mappers
{
    public class NewsMapper : IMapper<CreateNewsRequest, DbNews>
    {
        public DbNews Map(CreateNewsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbNews
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                Subject = request.Subject,
                AuthorName = request.AuthorName,
                AuthorId = request.AuthorId,
                SenderId = request.SenderId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}

