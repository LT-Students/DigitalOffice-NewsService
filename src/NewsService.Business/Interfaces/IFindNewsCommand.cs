using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  [AutoInject]
  public interface IFindNewsCommand
  {
    Task<FindResultResponse<NewsInfo>> ExecuteAsync(FindNewsFilter findNewsFilter);
  }
}
