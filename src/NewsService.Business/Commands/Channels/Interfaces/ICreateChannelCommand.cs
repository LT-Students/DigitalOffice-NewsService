using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces
{
  [AutoInject]
  public interface ICreateChannelCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateChannelRequest request);
  }
}
