using System.Collections.Generic;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Mappers.Responses
{
  public class NewsResponseMapper : INewsResponseMapper
  {
    public NewsResponse Map(
      DbNews dbNews,
      UserInfo creator,
      UserInfo publisher,
      ChannelInfo channel,
      List<TagInfo> tags)
    {
      if (dbNews is null)
      {
        return null;
      }

      return new NewsResponse
      {
        Id = dbNews.Id,
        Subject = dbNews.Subject,
        Preview = dbNews.Preview,
        Content = dbNews.Content,
        IsActive = dbNews.IsActive,
        CreatedAtUtc = dbNews.CreatedAtUtc,
        PublishedAtUtc = dbNews.PublishedAtUtc,
        Creator = creator,
        Publisher = publisher,
        Channel = channel,
        Tags = tags
      };
    }
  }
}
