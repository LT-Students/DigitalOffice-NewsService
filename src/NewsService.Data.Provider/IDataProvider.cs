using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Database;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    DbSet<DbNews> News { get; set; }
    DbSet<DbTag> Tags { get; set; }
    DbSet<DbChannel> Channels { get; set; }
    DbSet<DbNewsTags> NewsTags { get; set; }
  }
}
