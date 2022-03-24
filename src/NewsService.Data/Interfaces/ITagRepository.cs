using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface ITagRepository
  {
    Task<Guid?> CreateAsync(DbTag dbTag);

    Task<(List<DbTag> dbTags, int totalCount)> FindAsync(FindTagFilter filter);

    Task DowngradeCountAsync(List<Guid> tagsIds);

    Task UpdateCountAsync(List<Guid> tagsIds);
    
    Task RemoveAsync(DateTime time);
  }
}
