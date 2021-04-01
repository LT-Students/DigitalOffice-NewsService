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

        /*public List<DbNews> FindNews(FindNewsParams findNewsParams)
        {
           return provider.News
                .Where(aId => authorId == null || aId.AuthorId == authorId)
                .Where(dId => authorId == null || dId.AuthorId == authorId)
                .Where(aN => authorName == null || aN.AuthorName == authorName)
                .Where(nN => newsName == null || nN.Subject == newsName)
                .ToList();
        }*/
    }
}