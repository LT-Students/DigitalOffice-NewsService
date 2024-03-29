﻿using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces
{
  [AutoInject]
  public interface INewsResponseMapper
  {
    NewsResponse Map(
      DbNews dbNews,
      UserInfo creator,
      UserInfo publisher,
      ChannelInfo channel,
      List<TagInfo> tags);
  }
}
