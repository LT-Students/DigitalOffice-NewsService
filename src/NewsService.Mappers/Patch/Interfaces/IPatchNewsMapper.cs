using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchNewsMapper
  {
      JsonPatchDocument<DbNews> Map(JsonPatchDocument<EditNewsRequest> request);
  }
}
