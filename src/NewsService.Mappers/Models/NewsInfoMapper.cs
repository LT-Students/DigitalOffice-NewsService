using System;
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
    public NewsInfo Map(DbNews dbNews, DepartmentInfo department, UserInfo author, UserInfo sender)
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
        Pseudonym = dbNews.Pseudonym,
        Department = department,
        Author = author,
        IsActive = dbNews.IsActive,
        CreatedAtUtc = dbNews.CreatedAtUtc,
        Sender = sender
      };
    }
  }
}
