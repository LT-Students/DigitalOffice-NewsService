using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;

namespace LT.DigitalOffice.NewsService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbNewsTagMapper
  {
    List<DbNewsTag> Map(List<Guid> tagsIds, Guid newsId);
  }
}
