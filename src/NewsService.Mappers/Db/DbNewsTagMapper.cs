using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Db
{
  public class DbNewsTagMapper : IDbNewsTagMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbNewsTagMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public List<DbNewsTag> Map(List<Guid> tagsIds, Guid newsId)
    {
      return tagsIds.Select(tagId => new DbNewsTag
      {
        Id = Guid.NewGuid(),
        NewsId = newsId,
        TagId = tagId,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
      }).ToList();
    }
  }
}
