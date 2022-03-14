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
  public class TagRepository : ITagRepository
  {
    private readonly IDataProvider _provider;

    public TagRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<List<DbTag>> GetAsync(List<Guid> tagsIds)
    {
      return await _provider.Tags.Where(t => tagsIds.Contains(t.Id)).ToListAsync();
    }
  }
}
