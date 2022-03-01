using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface ITagsRepository
  {
    Task<List<DbTags>> GetAsync(List<Guid> tagId);
  }
}
