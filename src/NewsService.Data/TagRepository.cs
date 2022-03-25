using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data
{
  public class TagRepository : ITagRepository
  {
    private readonly IDataProvider _provider;

    public TagRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<Guid?> CreateAsync(DbTag dbTag)
    {
      if (dbTag is null)
      {
        return null;
      }

      _provider.Tags.Add(dbTag);
      await _provider.SaveAsync();

      return dbTag.Id;
    }

    public async Task DowngradeCountAsync(List<Guid> tagsIds)
    {
      IQueryable<DbTag> dbTagsList = _provider.Tags.AsQueryable();

      dbTagsList = dbTagsList.Where(t => tagsIds.Contains(t.Id));

      if (dbTagsList is not null && dbTagsList.Any())
      {
        foreach (DbTag dbTag in dbTagsList.Where(t => t.Count > 0))
        {
          dbTag.Count -= 1;
          dbTag.ModifiedAtUtc = DateTime.UtcNow;
        }

        await _provider.SaveAsync();
      }
    }

    public async Task UpdateCountAsync(List<Guid> tagsIds)
    {
      IQueryable<DbTag> dbTagsList = _provider.Tags.AsQueryable();

      dbTagsList = dbTagsList.Where(t => tagsIds.Contains(t.Id));

      if (dbTagsList is not null && dbTagsList.Any())
      {
        await dbTagsList.ForEachAsync(dbTag => dbTag.Count += 1);
        await _provider.SaveAsync();
      }
    }

    public async Task<(List<DbTag> dbTags, int totalCount)> FindAsync(FindTagFilter filter)
    {
      IQueryable<DbTag> dbTagsList = _provider.Tags.AsQueryable();

      return (
        await dbTagsList
          .OrderByDescending(n => n.Count)
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .ToListAsync(),
        await dbTagsList.CountAsync());
    }

    public async Task RemoveAsync()
    {
      List<DbTag> dbTags =
        _provider.Tags.Where(
          t => t.Count == 0 &&
          (DateTime.UtcNow.Hour - t.ModifiedAtUtc.Value.Hour == 0))
        .ToList();

      if (dbTags is not null && dbTags.Any())
      {
        _provider.Tags.RemoveRange(dbTags);
        await _provider.SaveAsync();
      }
    }
  }
}
