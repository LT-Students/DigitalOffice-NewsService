using LT.DigitalOffice.NewsService.Mappers.ModelMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.ModelMappers
{
    public class NewsMapper : INewsMapper
    {
        public DbNews Map(News request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var dbNews = new DbNews
            {
                Content = request.Content,
                Subject = request.Subject,
                Pseudonym= request.AuthorName,
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
    }
}

