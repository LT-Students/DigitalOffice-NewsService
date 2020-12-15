using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDataProvider provider;

        public NewsRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public DbNews GetNew(Guid newId)
        {
            var dbNews = provider.News.FirstOrDefault(x => x.Id == newId);

            if (dbNews == null)
            {
                throw new NullReferenceException("New with id does not exist");
            }

            return dbNews;
        }

        public void EditNews(DbNews news)
        {
            var dbNews = provider.News
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == news.Id);

            if (dbNews == null)
            {
                throw new Exception("News was not found.");
            }

            provider.News.Update(news);
            provider.Save();
        }

        public Guid CreateNews(DbNews news)
        {
            provider.News.Add(news);
            provider.Save();

            return news.Id;
        }

        public void CreateNewsHistory(DbNewsChangesHistory dbnewsHistory, Guid newId)
        {
            if (provider.News.Any(n => n.Id == newId))
            {
                throw new ArgumentException("Logging is not possible because news id does not exist");
            }

            provider.NewsHistory.Add(dbnewsHistory);
        }
    }
}