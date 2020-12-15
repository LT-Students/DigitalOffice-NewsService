using LT.DigitalOffice.NewsService.Mappers.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.NewsService.Mappers.RequestMappers.Interfaces
{
    public interface IAddChangeSetDetailsMapperRequest : IMapper<Operation<DbNews>, DbChangeSetDetails>
    {
    }
}
