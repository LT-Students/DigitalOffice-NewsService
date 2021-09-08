using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface INewsRepository
  {
    Guid? Create(DbNews news);

    bool Edit(Guid newsId, JsonPatchDocument<DbNews> news);

    DbNews Get(Guid newsId);

    List<DbNews> Find(FindNewsFilter findNewsFilter, List<string> errors, out int totalCount);
  }
}
