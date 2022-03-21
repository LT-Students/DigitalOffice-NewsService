using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data
{
  public class NewsTagRepository : INewsTagRepository
  {
    private readonly IDataProvider _provider;

    public NewsTagRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> DoNewsTagsIdsExist(Guid newsId, List<Guid> tagsIds)
    {
      return await _provider.NewsTags.AnyAsync(nt => nt.NewsId == newsId && tagsIds.Contains(nt.TagId));
    }

    public async Task RemoveAsync(Guid newsId, List<Guid> tagsIds)
    {
      List<DbNewsTag> dbNewsTag = await _provider.NewsTags
        .Where(nt => nt.NewsId == newsId && tagsIds.Contains(nt.TagId)).ToListAsync();

      if (dbNewsTag is not null && dbNewsTag.Any())
      {
        _provider.NewsTags.RemoveRange(dbNewsTag);
        await _provider.SaveAsync();
      }
    }

    public async Task CreateAsync(List<DbNewsTag> dbNewsTags)
    {
      _provider.NewsTags.AddRange(dbNewsTags);
      await _provider.SaveAsync();
    }
  }
}
