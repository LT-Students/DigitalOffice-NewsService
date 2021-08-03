using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IDbNewsMapper : IMapper<News, DbNews>
    {
    }
}
