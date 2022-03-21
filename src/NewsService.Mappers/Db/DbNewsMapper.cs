using System;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class DbNewsMapper : IDbNewsMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbNewsTagMapper _newsTagsMapper;
    public DbNewsMapper(
      IHttpContextAccessor httpContextAccessor,
      IDbNewsTagMapper newsTagsMapper)
    {
      _httpContextAccessor = httpContextAccessor;
      _newsTagsMapper = newsTagsMapper;
    }

    public DbNews Map(CreateNewsRequest request)
    {
      if (request is null)
      {
        return null;
      }
      
      Guid newsId = Guid.NewGuid();

      return new DbNews
      {
        Id = newsId,
        Preview = request.Preview,
        Content = request.Content,
        Subject = request.Subject,
        IsActive = request.IsActive,
        ChannelId = request.ChannelId,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        PublishedAtUtc = request.IsActive ? DateTime.UtcNow : null,
        PublishedBy = request.IsActive ? _httpContextAccessor.HttpContext.GetUserId() : null,
        //NewsTags = request.TagsIds.Any() ? _newsTagsMapper.Map(request.TagsIds.Distinct().ToList(), newsId) : null,
      };
    }
  }
}
