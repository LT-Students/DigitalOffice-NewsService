using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Tag;

namespace LT.DigitalOffice.NewsService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbTagMapper
  {
    DbTag Map(CreateTagRequest request);
  }
}
