using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Mappers
{
    public class NewsMapper : IMapper<DbNews, News>
    {
        public News Map(DbNews news)
        {
            if (news == null)
            {
                throw new ArgumentNullException(nameof(News));
            }
            return new News
            {
                Id = news.Id,
                Name = news.Name,
                Content = news.Content,
                PostTime = news.PostTime,
                IsActive = news.IsActive,
                UserId = news.UserId
            };
        }
    }
}
