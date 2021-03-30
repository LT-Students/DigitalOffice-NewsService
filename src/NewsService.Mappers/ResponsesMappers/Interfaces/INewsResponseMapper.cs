using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of News value <see cref="DbNews"/>
    /// </summary>
    public interface INewsResponseMapper : IMapper<DbNews, Task<NewsResponse>>
    {
    }
}
