using System;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Db
{
  public class DbTagMapper : IDbTagMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbTagMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbTag Map(CreateTagRequest request)
    {
      return new DbTag
      {
        Id = Guid.NewGuid(),
        Name = string.Join(" ", request.Name.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)),
        Count = 0,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
      };
    }
  }
}
