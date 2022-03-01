using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class ChannelInfoMapper : IChannelInfoMapper
  {
    public ChannelInfo Map(DbChannel dbChannel)
    {
      if (dbChannel is null)
      { 
        return null;
      }

      ImageConsist image = dbChannel.ImageContent is null
        ? null
        : new() { Content = dbChannel.ImageContent, Extension = dbChannel.ImageExtension };

      return new ChannelInfo
      {
        Id = dbChannel.Id,
        Name = dbChannel.Name,
        Image = image
      };
    }
  }
}
