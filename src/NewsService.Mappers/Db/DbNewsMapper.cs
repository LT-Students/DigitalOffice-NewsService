using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class DbNewsMapper : IDbNewsMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserInfoMapper _userInfoMapper;
    private readonly IDepartmentInfoMapper _departmentInfoMapper;

    public DbNewsMapper(
      IHttpContextAccessor httpContextAccessor,
      IUserInfoMapper userInfoMapper,
      IDepartmentInfoMapper departmentInfoMapper)
    {
      _httpContextAccessor = httpContextAccessor;
      _userInfoMapper = userInfoMapper;
      _departmentInfoMapper = departmentInfoMapper;
    }

    public DbNews Map(CreateNewsRequest request)
    {
      if (request == null)
      {
        return null;
      }

      return new DbNews
      {
        Id = Guid.NewGuid(),
        Preview = request.Preview,
        Content = request.Content,
        Subject = request.Subject,
        Pseudonym = !string.IsNullOrEmpty(request.Pseudonym?.Trim()) ? request.Pseudonym.Trim() : null,
        AuthorId = request.AuthorId,
        DepartmentId = request.DepartmentId,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
      };
    }
  }
}
