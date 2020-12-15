using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces
{
    public interface IEditNewsMapper : IMapper<JsonPatchDocument<DbNews>, DbNews>
    {
    }
}
