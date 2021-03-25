using LT.DigitalOffice.Kernel.Exceptions;
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
        private readonly IDataProvider _provider;

        public NewsRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void EditNews(DbNews news)
        {
            var dbNews = _provider.News
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == news.Id);

            if (dbNews == null)
            {
                throw new Exception("News was not found.");
            }

            _provider.News.Update(news);
            _provider.Save();
        }

        public Guid CreateNews(DbNews news)
        {
            _provider.News.Add(news);
            _provider.Save();

            return news.Id;
        }

        public DbNews GetNewsInfoById(Guid newsId)
            => _provider.News.FirstOrDefault(dbNews => dbNews.Id == newsId) ??
               throw new NotFoundException($"News with this id: '{newsId}' was not found.");
    }
}