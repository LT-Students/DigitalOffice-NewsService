using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface INewsTagRepository
  {
    Task<bool> DoNewsTagsIdsExist(Guid newsId, List<Guid> tagsIds);

    Task RemoveAsync(Guid newsId, List<Guid> tagsIds);

    Task CreateAsync(List<DbNewsTag> dbNewsTags);
  }
}
