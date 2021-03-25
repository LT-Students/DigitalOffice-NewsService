using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers
{
    public class NewsResponseMapper : INewsResponseMapper
    {
        public News Map(DbNews value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            return new News
            {
                Id = value.Id,
                Content = value.Content,
                Subject = value.Subject,
                AuthorName = value.AuthorName,
                AuthorId = value.AuthorId,
                SenderId = value.SenderId,
                CreatedAt = value.CreatedAt,
                IsActive = value.IsActive
            };
        }
    }
}
