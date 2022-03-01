using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Business.Commands.Channels.Interfaces
{
  [AutoInject]
  public interface IFindChannelCommand
  {
    Task<FindResultResponse<ChannelInfo>> ExecuteAsync(FindChannelFilter filter);
  }
}
