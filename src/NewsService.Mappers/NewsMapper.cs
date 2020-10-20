using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using System;

namespace LT.DigitalOffice.NewsService.Mappers
{
    public class NewsMapper : IMapper<NewsRequest, DbNews>, IMapper<DbNews, News>
    {
        public DbNews Map(NewsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var dbNews = new DbNews
            {
                Content = request.Content,
                Subject = request.Subject,
                AuthorName = request.AuthorName,
                AuthorId = request.AuthorId,
                SenderId = request.SenderId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            if (request.Id != null)
            {
                dbNews.Id = (Guid)request.Id;
            }
            else
            {
                dbNews.Id = Guid.NewGuid();
            }

            return dbNews;
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

