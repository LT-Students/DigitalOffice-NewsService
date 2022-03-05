using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Data.Interfaces
{
  [AutoInject]
  public interface IChannelRepository
  {
    Task<DbChannel> GetAsync(Guid channelId, GetChannelFilter filter);

    Task<Guid?> CreateAsync(DbChannel dbChannel);

    Task<(List<DbChannel> dbChannels, int totalCount)> FindAsync(FindChannelFilter filter);

    Task<bool> EditAsync(Guid channelId, JsonPatchDocument<DbChannel> patch);

    Task<bool> DoesNameExistAsync(string name);
  }
}
