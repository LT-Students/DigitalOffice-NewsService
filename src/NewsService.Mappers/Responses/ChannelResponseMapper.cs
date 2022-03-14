using System.Collections.Generic;
using LT.DigitalOffice.NewsService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Mappers.Responses
{
  public class ChannelResponseMapper : IChannelResponseMapper
  {
    public ChannelResponse Map(
      DbChannel dbChannel,
      List<NewsInfo> News)
    {
      if (dbChannel is null)
      {
        return null;
      }

      return new ChannelResponse
      {
        Id = dbChannel.Id,
        Name = dbChannel.Name,
        Image = dbChannel.ImageContent is null
          ? null
          : new() { Content = dbChannel.ImageContent, Extension = dbChannel.ImageExtension },
        News = News
      };
    }
  }
}
