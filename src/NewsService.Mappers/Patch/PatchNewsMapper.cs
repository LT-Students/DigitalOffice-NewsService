using LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.NewsService.Mappers.Patch
{
  public class PatchNewsMapper : IPatchNewsMapper
  {
    public JsonPatchDocument<DbNews> Map(JsonPatchDocument<EditNewsRequest> request)
    {
      if (request == null)
      {
        return null;
      }

      JsonPatchDocument<DbNews> patchDbNews = new JsonPatchDocument<DbNews>();

      foreach (Operation<EditNewsRequest> item in request.Operations)
      {
        patchDbNews.Operations.Add(new Operation<DbNews>(item.op, item.path, item.from, item.value));
      }

      return patchDbNews;
    }
  }
}
