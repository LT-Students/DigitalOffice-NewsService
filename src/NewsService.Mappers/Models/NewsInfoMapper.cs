using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class NewsInfoMapper : INewsInfoMapper
  {
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly IDepartmentInfoMapper _departmentInfoMapper;

    public NewsInfoMapper(IUserInfoMapper userInfoMapper, IDepartmentInfoMapper departmentInfoMapper)
    {
      _userInfoMapper = userInfoMapper;
      _departmentInfoMapper = departmentInfoMapper;
    }

    public NewsInfo Map(DbNews dbNews, List<DepartmentData> departments, List<UserData> authors)
    {
      if (dbNews == null)
      {
        return null;
      }

      List<ImageInfo> avatarImage = new();

      return new NewsInfo
      {
        Id = dbNews.Id,
        Preview = dbNews.Preview,
        Subject = dbNews.Subject,
        Departments = departments?.Select(_departmentInfoMapper.Map).ToList(),
        Authors = authors?.Select(a => _userInfoMapper.Map(a, avatarImage?.FirstOrDefault(i => i.Id == a.ImageId))).ToList(),
        IsActive = dbNews.IsActive,
        CreatedAtUtc = DateTime.UtcNow
      };
    }
  }
}
