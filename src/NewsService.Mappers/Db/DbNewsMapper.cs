﻿using System;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
  public class DbNewsMapper : IDbNewsMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbNewsMapper(
      IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public DbNews Map(CreateNewsRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbNews
      {
        Id = Guid.NewGuid(),
        Preview = request.Preview,
        Content = request.Content,
        Subject = request.Subject,
        IsActive = request.IsActive,
        ChannelId = request.ChannelId,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        PublishedAtUtc = request.IsActive ? DateTime.UtcNow : null,
        PublishedBy = request.IsActive ? _httpContextAccessor.HttpContext.GetUserId() : null,
      };
    }
  }
}
