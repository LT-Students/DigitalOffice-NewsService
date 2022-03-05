using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;

namespace LT.DigitalOffice.NewsService.Business.Commands.News.Interfaces
{
  [AutoInject]
  public interface ICreateNewsCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateNewsRequest request);
  }
}
