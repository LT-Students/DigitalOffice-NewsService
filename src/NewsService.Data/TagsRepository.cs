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
  public class TagsRepository : ITagsRepository
  {
    private readonly IDataProvider _provider;

    public TagsRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<List<DbTags>> GetAsync(List<Guid> tagId)
    {
      return await _provider.Tags.Where(t => tagId.Contains(t.Id)).ToListAsync();
    }
  }
}
