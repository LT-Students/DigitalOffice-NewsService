using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.ModelResponse;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using System.Threading.Tasks;

namespace LT.DigitalOffice.NewsService.Mappers.ResponsesMappers.Interface
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="DbNews"/>
    /// </summary>
    public interface INewsResponseMapper : IMapper<DbNews, NewsResponse>
    {
    }
}
