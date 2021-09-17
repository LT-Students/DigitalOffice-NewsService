using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Responses
{
  public class NewsResponseMapper : INewsResponseMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserInfoMapper _userInfoMapper;

    public NewsResponseMapper(IHttpContextAccessor httpContextAccessor, IUserInfoMapper userInfoMapper)
    {
      _httpContextAccessor = httpContextAccessor;
      _userInfoMapper = userInfoMapper;
    }

    public NewsResponse Map(DbNews dbNews, DepartmentInfo department, UserData author)
    {
      if (dbNews == null)
      {
        return null;
      }

      List<ImageInfo> avatarImage = new();

      return new NewsResponse
      {
        Id = dbNews.Id,
        Preview = dbNews.Preview,
        Content = dbNews.Content,
        Subject = dbNews.Subject,
        Pseudonym = dbNews.Pseudonym,
        Author = _userInfoMapper.Map(author, avatarImage?.FirstOrDefault(ai => ai.Id == author.ImageId)),
        Department = department,
        IsActive = dbNews.IsActive,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId()
      };
    }
  }
}
