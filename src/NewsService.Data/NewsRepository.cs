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

    private IQueryable<DbNews> CreateFindPredicates(
      FindNewsFilter findNewsFilter,
      IQueryable<DbNews> dbNewsList)
    {
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

      return dbNewsList;
    }

    public NewsRepository(IDataProvider provider, IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public bool Edit(DbNews dbNews, JsonPatchDocument<DbNews> request)
    {
      if (dbNews == null || request == null)
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

      IQueryable<DbNews> dbNewsList = _provider.News.AsQueryable();

      CreateFindPredicates(findNewsFilter, dbNewsList).FirstOrDefault();

      totalCount = dbNewsList.Count();

      return dbNewsList.Skip(findNewsFilter.skipCount).Take(findNewsFilter.takeCount).ToList();
    }

    public DbNews Get(Guid newsId)
    {
      return _provider.News.FirstOrDefault(dbNews => dbNews.Id == newsId);
    }
  }
}
