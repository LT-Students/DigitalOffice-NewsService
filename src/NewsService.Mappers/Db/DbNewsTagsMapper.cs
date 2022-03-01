using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Db
{
  public class DbNewsTagsMapper : IDbNewsTagsMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbNewsTagsMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public List<DbNewsTags> Map(List<Guid> tagsIds, Guid newsIds)
    {
      return tagsIds.Select(tagId => new DbNewsTags
      {
        Id = Guid.NewGuid(),
        NewsId = newsIds,
        TagId = tagId,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
      }).ToList();
    }
  }
}
