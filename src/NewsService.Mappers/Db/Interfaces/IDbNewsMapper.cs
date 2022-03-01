using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;

namespace LT.DigitalOffice.NewsService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface IDbNewsMapper
  {
    DbNews Map(CreateNewsRequest request);
  }
}
