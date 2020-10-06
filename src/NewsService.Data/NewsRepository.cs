using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.DigitalOffice.NewsService.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDataProvider provider;

        public NewsRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public DbNews GetNewsById(Guid newsId)
        {
            var dbNews = provider.News.FirstOrDefault(x => x.Id == newsId);
            if (dbNews == null)
            {
                throw new Exception("News was not found.");
            }

            return dbNews;
        }
    }
}
