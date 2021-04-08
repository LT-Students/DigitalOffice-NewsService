using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Responses;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of News value <see cref="DbNews"/>
    /// </summary>
    [AutoInject]
    public interface INewsResponseMapper : IMapper<DbNews, NewsResponse>
    {
    }
}
