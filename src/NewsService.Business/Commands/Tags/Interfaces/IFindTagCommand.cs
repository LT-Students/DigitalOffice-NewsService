using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Business.Commands.Tags.Interfaces
{
  [AutoInject]
  public interface IFindTagCommand
  {
    Task<FindResultResponse<TagInfo>> ExecuteAsync(FindTagFilter filter);
  }
}
