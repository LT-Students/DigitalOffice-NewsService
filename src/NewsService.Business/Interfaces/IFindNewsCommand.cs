using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  [AutoInject]
  public interface IFindNewsCommand
  {
    /// <summary>
    /// Fined news.
    /// </summary>
    /// <param name="findNewsFilter">Model witch filtres.</param>
    FindResultResponse<NewsInfo> Execute(FindNewsFilter findNewsFilter, int skipCount, int takeCount);
  }
}
