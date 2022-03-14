using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface INewsTagRepository
  {
    void CreateAsync(List<DbNewsTag> dbNewsTags);
  }
}
