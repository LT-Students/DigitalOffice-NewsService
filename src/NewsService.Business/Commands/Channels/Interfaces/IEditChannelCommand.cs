using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces
{
  [AutoInject]
  public interface IEditChannelCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid chanelId, JsonPatchDocument<EditChannelRequest> request);
  }
}
