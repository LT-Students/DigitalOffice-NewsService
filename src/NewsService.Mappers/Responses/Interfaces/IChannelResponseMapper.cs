using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Mappers.Responses.Interfaces
{
  [AutoInject]
  public interface IChannelResponseMapper
  {
    ChannelResponse Map(DbChannel dbChannel, List<NewsInfo> News);
  }
}
