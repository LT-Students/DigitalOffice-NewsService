using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.NewsService.Mappers.Patch
{
  public class PatchNewsMapper : IPatchNewsMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PatchNewsMapper(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public JsonPatchDocument<DbNews> Map(JsonPatchDocument<EditNewsRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbNews> patchDbNews = new JsonPatchDocument<DbNews>();

      foreach (Operation<EditNewsRequest> item in request.Operations)
      {
        if (item.path.Contains("/IsActive") && item.value.Equals(true))
        {
          patchDbNews.Operations.Add(new Operation<DbNews>(item.op, item.path, item.from, item.value));
          
          patchDbNews.Operations.Add(
            new Operation<DbNews>(
              item.op = "replace",
              item.path = "/PublishedBy",
              item.from,
              item.value = _httpContextAccessor.HttpContext.GetUserId()));

          patchDbNews.Operations.Add(
            new Operation<DbNews>(
              item.op = "replace",
              item.path = "/PublishedAtUtc",
              item.from,
              item.value = DateTime.UtcNow));
        }

        patchDbNews.Operations.Add(new Operation<DbNews>(item.op, item.path, item.from, item.value));
      }

      return patchDbNews;
    }
  }
}
