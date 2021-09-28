﻿using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class NewsInfoMapper : INewsInfoMapper
  {
    public NewsInfo Map(DbNews dbNews, DepartmentInfo department, UserInfo author)
    {
      if (dbNews == null)
      {
        return null;
      }

      return new NewsInfo
      {
        Id = dbNews.Id,
        Preview = dbNews.Preview,
        Subject = dbNews.Subject,
        Department = department,
        Author = author,
        IsActive = dbNews.IsActive,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
