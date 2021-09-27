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
    private readonly IUserInfoMapper _userInfoMapper;

    public NewsInfoMapper(IUserInfoMapper userInfoMapper)
    {
      _userInfoMapper = userInfoMapper;
    }

    public NewsInfo Map(DbNews dbNews, DepartmentInfo department, UserData author, List<ImageData> avatarImage)
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
        Author = _userInfoMapper.Map(author, avatarImage?.FirstOrDefault(i => i.ImageId == author.ImageId)),
        IsActive = dbNews.IsActive,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
