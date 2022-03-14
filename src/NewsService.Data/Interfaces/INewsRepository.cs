using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface INewsRepository
  {
    Task<Guid?> CreateAsync(DbNews dbNews);

    Task<bool> EditAsync(Guid newsId, JsonPatchDocument<DbNews> patch);

    Task<DbNews> GetAsync(Guid newsId);

    Task<List<DbNews>> GetAsync(List<Guid> newsIds);

    Task<(List<DbNews> dbNews, int totalCount)> FindAsync(FindNewsFilter filter);

    Task<List<DbNews>> SearchAsync(string text);

    Task<bool> DoesNewsExistAsync(Guid newsId);
  }
}
