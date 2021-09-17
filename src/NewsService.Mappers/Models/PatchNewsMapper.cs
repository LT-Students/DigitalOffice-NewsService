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
        throw new ArgumentNullException("Invalid request value");
      }

      var patchDbNews = new JsonPatchDocument<DbNews>();

      foreach (var item in request.Operations)
      {
        if (item.op == "replace")
        {
          patchDbNews.Operations.Add(new Operation<DbNews>(item.op, item.path, item.from, item.value));
        }
        else
        {
          throw new ArgumentException("Invalid operation");
        }
      }

      return patchDbNews;
    }
  }
}
