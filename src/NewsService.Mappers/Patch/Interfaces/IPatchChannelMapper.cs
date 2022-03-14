using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces
{
  [AutoInject]
  public interface IPatchChannelMapper
  {
    Task<JsonPatchDocument<DbChannel>> MapAsync(JsonPatchDocument<EditChannelRequest> request);
  }
}
