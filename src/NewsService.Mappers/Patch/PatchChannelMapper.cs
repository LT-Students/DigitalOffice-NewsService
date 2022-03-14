using System;
using System.Threading.Tasks;
using LT.DigitalOffice.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.NewsService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Models;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;

namespace LT.DigitalOffice.NewsService.Mappers.Patch
{
  public class PatchChannelMapper : IPatchChannelMapper
  {
    private readonly IImageResizeHelper _resizeHelper;

    public PatchChannelMapper(IImageResizeHelper resizeHelper)
    {
      _resizeHelper = resizeHelper;
    }
    public async Task<JsonPatchDocument<DbChannel>> MapAsync(JsonPatchDocument<EditChannelRequest> request)
    {
      if (request is null)
      {
        return null;
      }

      JsonPatchDocument<DbChannel> patchDbNews = new JsonPatchDocument<DbChannel>();

      foreach (var item in request.Operations)
      {
        if (item.path.EndsWith(nameof(EditChannelRequest.Image), StringComparison.OrdinalIgnoreCase))
        {
          ImageConsist image = JsonConvert.DeserializeObject<ImageConsist>(item.value.ToString());
          (bool _, string resizedContent, string extension) = await _resizeHelper.ResizeAsync(
            image.Content, image.Extension);
          patchDbNews.Operations.Add(new Operation<DbChannel>(item.op, nameof(DbChannel.ImageContent), item.from, resizedContent));
          patchDbNews.Operations.Add(new Operation<DbChannel>(item.op, nameof(DbChannel.ImageExtension), item.from, extension));

          continue;
        }

        patchDbNews.Operations.Add(new Operation<DbChannel>(item.op, item.path, item.from, item.value));
      }

      return patchDbNews;
    }
  }
}
