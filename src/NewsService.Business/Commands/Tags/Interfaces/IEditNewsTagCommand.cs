using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;

namespace LT.DigitalOffice.NewsService.Business.Commands.Tags.Interfaces
{
  [AutoInject]
  public interface IEditNewsTagCommand
  {
    Task<OperationResultResponse<bool>> ExecuteAsync(Guid newsId, EditNewsTagsRequest request);
  }
}
