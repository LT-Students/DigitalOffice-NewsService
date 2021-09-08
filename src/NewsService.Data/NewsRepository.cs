using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Exceptions.Models;
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

    public List<DbNews> Find(FindNewsFilter findNewsFilter, int skipCount, int takeCount, List<string> errors, out int totalCount)
    {
      totalCount = 0;

      if (findNewsFilter == null)
      {
        return null;
      }

      if (skipCount < 0)
      {
        errors.Add("Skip count can't be less than 0.");
        return null;
      }

      if (takeCount < 1)
      {
        errors.Add("Take count can't be less than 1.");
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

      return dbNewsList.Skip(skipCount).Take(takeCount).ToList();
    }

    public DbNews Get(Guid newsId)
    {
      return _provider.News.FirstOrDefault(dbNews => dbNews.Id == newsId);
    }
  }
}
