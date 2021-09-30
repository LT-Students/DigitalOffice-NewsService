using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Business.Interfaces
{
  [AutoInject]
  public interface IEditNewsCommand
  {
    OperationResultResponse<bool> Execute(Guid newsId, JsonPatchDocument<EditNewsRequest> request);
  }
}
