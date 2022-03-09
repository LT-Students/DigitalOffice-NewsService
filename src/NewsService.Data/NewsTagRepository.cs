using System.Collections.Generic;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;

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

    public async void CreateAsync(List<DbNewsTag> dbNewsTags)
    {
      _provider.NewsTags.AddRange(dbNewsTags);
      await _provider.SaveAsync();
    }
  }
}
