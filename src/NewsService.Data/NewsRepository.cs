using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

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

    public bool Edit(Guid newsId, JsonPatchDocument<DbNews> request)
    {
      var dbNews = _provider.News.FirstOrDefault(x => x.Id == newsId);

      if (dbNews == null)
      {
        return false;
      }

      request.ApplyTo(dbNews);
      dbNews.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbNews.ModifiedAtUtc = DateTime.UtcNow;
      _provider.Save();

      return true;
    }

    public Guid? Create(DbNews news)
    {
      if (news == null)
      {
        return null;
      }

      _provider.News.Add(news);
      _provider.Save();

      return news.Id;
    }

    public List<DbNews> Find(FindNewsFilter findNewsFilter, out int totalCount)
    {
      if (findNewsFilter == null)
      {
        totalCount = 0;
        return null;
      }

      if (findNewsFilter.SkipCount < 0)
      {
        totalCount = 0;
        return null;
      }

      if (findNewsFilter.TakeCount < 1)
      {
        totalCount = 0;
        return null;
      }

      var dbNewsList = _provider.News.AsQueryable();

      if (findNewsFilter.AuthorId.HasValue)
      {
        dbNewsList = dbNewsList.Where(x => x.AuthorId == findNewsFilter.AuthorId);
      }

      if (findNewsFilter.DepartmentId.HasValue)
      {
        dbNewsList = dbNewsList.Where(x => x.DepartmentId == findNewsFilter.DepartmentId);
      }

      if (!findNewsFilter.IncludeDeactivated)
      {
        dbNewsList = dbNewsList.Where(x => x.IsActive);
      }

      totalCount = dbNewsList.Count();

      return dbNewsList.Skip(findNewsFilter.SkipCount).Take(findNewsFilter.TakeCount).ToList();
    }

    public DbNews Get(Guid newsId)
    {
      return _provider.News.FirstOrDefault(dbNews => dbNews.Id == newsId);
    }
  }
}
