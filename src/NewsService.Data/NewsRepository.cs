﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data
{
  public class NewsRepository : INewsRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NewsRepository(IDataProvider provider, IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> EditAsync(Guid newsId, JsonPatchDocument<DbNews> patch)
    {
      DbNews dbNews = await _provider.News.FirstOrDefaultAsync(x => x.Id == newsId);

      if (dbNews == null || patch == null)
      {
        return false;
      }

      patch.ApplyTo(dbNews);
      dbNews.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbNews.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }

    public async Task<Guid?> CreateAsync(DbNews dbNews)
    {
      if (dbNews == null)
      {
        return null;
      }

      _provider.News.Add(dbNews);
      await _provider.SaveAsync();

      return dbNews.Id;
    }

    public async Task<(List<DbNews> dbNewsList, int totalCount)> FindAsync(FindNewsFilter filter)
    {
      if (filter == null)
      {
        return (null, default);
      }

      IQueryable<DbNews> dbNewsList = _provider.News.AsQueryable();

      if (filter.AuthorId.HasValue)
      {
        dbNewsList = dbNewsList.Where(x => x.AuthorId == filter.AuthorId);
      }

      if (!filter.IncludeDeactivated)
      {
        dbNewsList = dbNewsList.Where(x => x.IsActive);
      }

      return (
        await dbNewsList
          .OrderByDescending(n => n.CreatedAtUtc)
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .ToListAsync(),
        await dbNewsList.CountAsync());
    }

    public async Task<DbNews> GetAsync(Guid newsId)
    {
      return await _provider.News.FirstOrDefaultAsync(dbNews => dbNews.Id == newsId);
    }

    public async Task<List<DbNews>> GetAsync(List<Guid> newsIds)
    {
      if (newsIds is null)
      {
        return null;
      }

      IQueryable<DbNews> dbNews = _provider.News.AsQueryable();

      return await dbNews
        .Where(x => newsIds.Contains(x.Id))
        .ToListAsync();
    }

    public async Task<List<DbNews>> SearchAsync(string text)
    {
      List<DbNews> dbNews = await _provider.News.ToListAsync();

      return await Task.Run(() =>
      {
        return dbNews
          .Where(n => n.Subject.Contains(text, StringComparison.OrdinalIgnoreCase))
          .ToList();
      });
    }
  }
}
