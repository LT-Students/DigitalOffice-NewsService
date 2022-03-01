using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Business.Commands.News.Interfaces
{
  [AutoInject]
  public interface IGetNewsCommand
  {
    Task<OperationResultResponse<NewsResponse>> ExecuteAsync(Guid newsId);
  }
}
