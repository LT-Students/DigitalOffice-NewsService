using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.NewsService.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDataProvider _provider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsRepository(
            IDataProvider provider,
            IHttpContextAccessor httpContextAccessor)
        {
            _provider = provider;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool EditNews(Guid newsId, JsonPatchDocument<DbNews> news)
        {
            var dbNews = _provider.News.FirstOrDefault(x => x.Id == newsId);

            if (dbNews == null)
            {
                throw new NotFoundException("News was not found.");
            }

            news.ApplyTo(dbNews);
            dbNews.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
            dbNews.ModifiedAtUtc = DateTime.UtcNow;
            _provider.Save();

            return true;
        }

        public Guid CreateNews(DbNews news)
        {
            _provider.News.Add(news);
            _provider.Save();

            return news.Id;
        }

        public List<DbNews> FindNews(FindNewsFilter findNewsFilter)
        {
            if (findNewsFilter == null)
            {
                throw new ArgumentNullException("search parameters not passed.");
            }

            var dbNewsList = _provider.News.AsQueryable();

            if (findNewsFilter.AuthorId != null)
            {
                dbNewsList = dbNewsList.Where(x => x.AuthorId == findNewsFilter.AuthorId);
            }

            if (findNewsFilter.DepartmentId != null)
            {
                dbNewsList = dbNewsList.Where(x => x.DepartmentId == findNewsFilter.DepartmentId);
            }

            if (findNewsFilter.Pseudonym != null)
            {
                dbNewsList = dbNewsList.Where(x => x.Pseudonym == findNewsFilter.Pseudonym);
            }

            if (findNewsFilter.Subject != null)
            {
                dbNewsList = dbNewsList.Where(x => x.Subject == findNewsFilter.Subject);
            }

            return dbNewsList.ToList();
        }

        public DbNews GetNewsInfoById(Guid newsId)
           => _provider.News.FirstOrDefault(dbNews => dbNews.Id == newsId) ??
              throw new NotFoundException($"News with this id: '{newsId}' was not found.");
    }
}