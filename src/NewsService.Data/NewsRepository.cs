using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using System;

namespace LT.DigitalOffice.NewsService.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDataProvider provider;

        public NewsRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public Guid CreateNews(DbNews news)
        {
            provider.News.Add(news);
            provider.Save();

            return news.Id;
        }
    }
}
