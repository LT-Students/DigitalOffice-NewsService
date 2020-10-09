using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using System;

namespace LT.DigitalOffice.NewsService.Mappers
{
    public class NewsMapper : IMapper<CreateNewsRequest, DbNews>, IMapper<DbNews, News>
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

        public News Map(DbNews dbNews)
        {
            if (dbNews == null)
            {
                throw new ArgumentNullException(nameof(dbNews));
            }

            return new News
            {
                Id = dbNews.Id,
                Content = dbNews.Content,
                Subject = dbNews.Subject,
                AuthorName = dbNews.AuthorName,
                AuthorId = dbNews.AuthorId,
                SenderId = dbNews.SenderId,
                CreatedAt = dbNews.CreatedAt,
                IsActive = dbNews.IsActive
            };
        }
    }
}

