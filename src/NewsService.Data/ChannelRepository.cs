﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Data.Interfaces;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.NewsService.Data
{
  public class ChannelRepository : IChannelRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChannelRepository(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DbChannel> GetAsync(Guid channelId, GetChannelFilter filter)
    {
      IQueryable<DbChannel> dbChannel = _provider.Channels.AsQueryable();

      if (filter.IncludeNews)
      {
        dbChannel = dbChannel
          .Include(c => c.News.Where(n => n.IsActive)
          .OrderByDescending(n => n.PublishedAtUtc)
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount));
      }

      return await dbChannel.FirstOrDefaultAsync(c => c.Id == channelId);
    }

    public async Task<bool> EditAsync(Guid channelId, JsonPatchDocument<DbChannel> patch)
    {
      DbChannel dbChannel = await _provider.Channels.FirstOrDefaultAsync(x => x.Id == channelId);

      if (dbChannel is null || patch is null)
      {
        return false;
      }

      patch.ApplyTo(dbChannel);
      dbChannel.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbChannel.ModifiedAtUtc = DateTime.UtcNow;
      await _provider.SaveAsync();

      return true;
    }

    public async Task<Guid?> CreateAsync(DbChannel dbChannel)
    {
      if (dbChannel is null)
      {
        return default;
      }

      _provider.Channels.Add(dbChannel);
      await _provider.SaveAsync();

      return dbChannel.Id;
    }

    public async Task<(List<DbChannel> dbChannels, int totalCount)> FindAsync(FindChannelFilter filter)
    {
      if (filter is null)
      {
        return (null, default);
      }

      IQueryable<DbChannel> dbChannels = _provider.Channels.AsQueryable();

      if (filter.Name is not null)
      {
        dbChannels = dbChannels.Where(c => c.Name == filter.Name);
      }

      if (filter.IsActive.HasValue)
      {
        dbChannels = dbChannels.Where(c => c.IsActive == filter.IsActive);
      }

      return(
        await dbChannels
          .OrderByDescending(n => n.Name)
          .Skip(filter.SkipCount)
          .Take(filter.TakeCount)
          .ToListAsync(),
        await dbChannels.CountAsync());
    }

    public async Task<bool> DoesNameExistAsync(string name)
    {
      return await _provider.Channels.AnyAsync(c => c.Name == name);
    }

    public async Task<bool> DoesChannelExistAsync(Guid channelId)
    {
      return await _provider.Channels.AnyAsync(c => c.Id == channelId);
    }
  }
}
