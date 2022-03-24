using System;
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

    private IQueryable<DbNews> CreateFindPredicates(
      FindNewsFilter filter,
      IQueryable<DbNews> dbNewsList)
    {
      if (filter.PublisherId.HasValue)
      {
        dbNewsList = dbNewsList.Where(nl => nl.PublishedBy == filter.PublisherId);
      }

      if (!filter.IncludeDeactivated)
      {
        dbNewsList = dbNewsList.Where(nl => nl.IsActive);
      }

      if (filter.IncludeChannel)
      {
        dbNewsList = dbNewsList.Where(nl => nl.ChannelId.HasValue);

        dbNewsList = dbNewsList.Include(nl => nl.Channel);
      }

      if (filter.ChannelId.HasValue)
      {
        dbNewsList = dbNewsList.Where(nl => nl.ChannelId == filter.ChannelId);
      }

      if (filter.TagId.HasValue)
      {
        dbNewsList = dbNewsList.Where(nl => nl.NewsTags.Select(nt => nt.TagId).Contains(filter.TagId.Value));

        dbNewsList = dbNewsList.Include(nl => nl.NewsTags).ThenInclude(nt => nt.Tag);
      }

      return dbNewsList;
    }

    public NewsRepository(IDataProvider provider, IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> EditAsync(Guid newsId, JsonPatchDocument<DbNews> patch)
    {
      DbNews dbNews = await _provider.News.FirstOrDefaultAsync(x => x.Id == newsId);

      if (dbNews is null || patch is null)
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
      if (dbNews is null)
      {
        return null;
      }

      _provider.News.Add(dbNews);
      await _provider.SaveAsync();

      return dbNews.Id;
    }

    public async Task<(List<DbNews> dbNews, int totalCount)> FindAsync(FindNewsFilter filter)
    {
      if (filter is null)
      {
        return (null, default);
      }

      IQueryable<DbNews> dbNewsList = CreateFindPredicates(
        filter,
        _provider.News.AsQueryable());

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
      IQueryable<DbNews> dbNews = _provider.News.AsQueryable();

      dbNews = dbNews.Where(dbNews => dbNews.Id == newsId);

      return await dbNews.Include(dbNews => dbNews.Channel).Include(dbNews => dbNews.NewsTags).ThenInclude(nt => nt.Tag).FirstOrDefaultAsync();
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

    public async Task<bool> DoesNewsExistAsync(Guid newsId)
    {
      return await _provider.News.AnyAsync(n => n.Id == newsId);
    }
  }
}
