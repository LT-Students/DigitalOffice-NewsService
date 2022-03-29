using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class NewsInfoMapper : INewsInfoMapper
  {
    private readonly ITagInfoMapper _tagsInfoMapper;

    public NewsInfoMapper(
      ITagInfoMapper tagsInfoMapper)
    {
      _tagsInfoMapper = tagsInfoMapper;
    }

    public NewsInfo Map(
      DbNews dbNews,
      List<UserInfo> users,
      ChannelInfo channel = null)
    {
      if (dbNews is null)
      {
        return null;
      }

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
        Tags = _tagsInfoMapper.Map(dbNews.NewsTags.Select(x => x.Tag).ToList())
      };
    }
  }
}
