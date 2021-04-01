using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public List<DbNews> FindNews(FindNewsParams findNewsParams)
        {
            var dbNewsList = provider.News.AsQueryable();
            if (findNewsParams.AuthorId != null)
            {
                dbNewsList = dbNewsList.Where(x => x.AuthorId == findNewsParams.AuthorId);
            }

            if (findNewsParams.DepartmentId != null)
            {
                dbNewsList = dbNewsList.Where(x => x.DepartmentId == findNewsParams.DepartmentId);
            }

            if (findNewsParams.AuthorPseudonym != null)
            {
                dbNewsList = dbNewsList.Where(x => x.AuthorName == findNewsParams.AuthorPseudonym);
            }

            if (findNewsParams.Subject != null)
            {
                dbNewsList = dbNewsList.Where(x => x.Subject == findNewsParams.Subject);
            }

            return dbNewsList.ToList();
        }
    }
}