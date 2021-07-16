using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.NewsService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IPatchNewsMapper
    {
        JsonPatchDocument<DbNews> Map(JsonPatchDocument<EditNewsRequest> request);
    }
}
