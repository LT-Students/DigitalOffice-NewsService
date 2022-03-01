using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces
{
  [AutoInject]
  public interface IGetChannelCommand
  {
    Task<OperationResultResponse<ChannelResponse>> ExecuteAsync(Guid channelId);
  }
}
