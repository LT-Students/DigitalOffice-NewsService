using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;

namespace LT.DigitalOffice.NewsService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbChannelMapper
  {
    Task<DbChannel> MapAsync(CreateChannelRequest request);
  }
}
