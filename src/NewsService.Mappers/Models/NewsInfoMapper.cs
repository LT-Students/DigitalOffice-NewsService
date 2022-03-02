using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class NewsInfoMapper : INewsInfoMapper
  {
    private readonly ITagsInfoMapper _tagsInfoMapper;
    public NewsInfo Map(
      DbNews dbNews,
      List<UserInfo> users,
      ChannelInfo channel)
    {
      return new NewsInfo
      {
        Id = dbNews.Id,
        Preview = dbNews.Preview,
        Subject = dbNews.Subject,
        IsActive = dbNews.IsActive,
        CreatedAtUtc = dbNews.CreatedAtUtc,
        PublishedAtUtc = dbNews.PublishedAtUtc,
        Publisher = users.Where(u => u.Id == dbNews.PublishedBy).FirstOrDefault(),
        Creator = users.Where(u => u.Id == dbNews.CreatedBy).FirstOrDefault(),
        Channel = channel,
        Tags = _tagsInfoMapper.Map(dbNews.Tags.ToList())
      };
    }

    public List<NewsInfo> Map(
      List<DbNews> dbNews,
      List<UserInfo> users)
    {
      return dbNews.Select(n => new NewsInfo
      {
        Id = n.Id,
        Preview = n.Preview,
        Subject = n.Subject,
        IsActive = n.IsActive,
        CreatedAtUtc = n.CreatedAtUtc,
        PublishedAtUtc = n.PublishedAtUtc,
        Publisher = users.Where(u => n.PublishedBy == u.Id).FirstOrDefault(),
        Creator = users.Where(u => n.CreatedBy == u.Id).FirstOrDefault(),
        Tags = _tagsInfoMapper.Map(n.Tags.ToList()) 
      }).ToList();
    }
    public NewsInfoMapper(
      ITagsInfoMapper tagsInfoMapper)
    {
      _tagsInfoMapper = tagsInfoMapper;
    }
  }
}
