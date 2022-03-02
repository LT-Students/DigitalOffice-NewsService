using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;

namespace LT.DigitalOffice.NewsService.Data
{
  public class NewsTagsRepository : INewsTagsRepository
  {
    private readonly IDataProvider _provider;

    public NewsTagsRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public async void CreateAsync(List<DbNewsTags> dbNewsTags)
    {
      dbNewsTags.Select(nt => _provider.NewsTags.Add(nt));
      await _provider.SaveAsync();
    }
  }
}
