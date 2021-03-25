using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="DbNews"/>
    /// type into an object of <see cref="News"/> type according to some rule.
    /// </summary>
    public interface INewsResponseMapper : IMapper<DbNews, News>
    {
    }
}
