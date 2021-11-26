using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  [AutoInject]
  public interface IEditNewsCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(
      Guid newsId,
      JsonPatchDocument<EditNewsRequest> request);
  }
}
