﻿using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.NewsService.Mappers.Models
{
    public class DbNewsMapper : IDbNewsMapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbNewsMapper (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbNews Map(News request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbNews
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                Subject = request.Subject,
                Pseudonym = !string.IsNullOrEmpty(request.Pseudonym?.Trim()) ? request.Pseudonym.Trim() : null,
                AuthorId = request.AuthorId,
                DepartmentId = request.DepartmentId,
                IsActive = true,
                CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}