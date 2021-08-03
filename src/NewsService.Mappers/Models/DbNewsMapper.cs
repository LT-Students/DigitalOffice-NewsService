using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
    public class DbNewsMapper : IDbNewsMapper
    {
        public DbNews Map(News request)
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
                Pseudonym = !string.IsNullOrEmpty(request.Pseudonym?.Trim()) ? request.Pseudonym.Trim() : null,
                AuthorId = request.AuthorId,
                SenderId = request.SenderId,
                DepartmentId = request.DepartmentId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
    }
}

