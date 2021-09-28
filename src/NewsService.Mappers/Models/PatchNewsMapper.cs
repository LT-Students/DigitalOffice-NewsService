using System;
using LT.DigitalOffice.NewsService.Mappers.Models.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.NewsService.Mappers.Models
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
