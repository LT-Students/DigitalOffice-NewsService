using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  [AutoInject]
  public interface ICreateNewsCommand
  {
    Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateNewsRequest request);
  }
}
