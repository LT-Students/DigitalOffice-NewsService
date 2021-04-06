using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public List<DbNews> FindNews(FindNewsParams findNewsParams)
        {
            if (findNewsParams == null)
            {
                throw new ArgumentNullException("search parameters not passed.");
            }

            var dbNewsList = _provider.News.AsQueryable();

            if (findNewsParams.AuthorId != null)
            {
                dbNewsList = dbNewsList.Where(x => x.AuthorId == findNewsParams.AuthorId);
            }

            if (findNewsParams.DepartmentId != null)
            {
                dbNewsList = dbNewsList.Where(x => x.DepartmentId == findNewsParams.DepartmentId);
            }

            if (findNewsParams.Pseudonym != null)
            {
                dbNewsList = dbNewsList.Where(x => x.Pseudonym == findNewsParams.Pseudonym);
            }

            if (findNewsParams.Subject != null)
            {
                dbNewsList = dbNewsList.Where(x => x.Subject == findNewsParams.Subject);
            }

            return dbNewsList.ToList();
        }

        public DbNews GetNewsInfoById(Guid newsId)
           => _provider.News.FirstOrDefault(dbNews => dbNews.Id == newsId) ??
              throw new NotFoundException($"News with this id: '{newsId}' was not found.");
    }
}