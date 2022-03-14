using System;
using System.Threading.Tasks;
using LT.DigitalOffice.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Mappers.Db.Interfaces;
using LT.DigitalOffice.NewsService.Models.Db;
using LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Mappers.Db
{
  public class DbChanelMapper : IDbChannelMapper
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageResizeHelper _resizeHelper;

    public DbChanelMapper(
      IHttpContextAccessor httpContextAccessor,
      IImageResizeHelper resizeHelper)
    {
      _httpContextAccessor = httpContextAccessor;
      _resizeHelper = resizeHelper;
    }

    public async Task<DbChannel> MapAsync(CreateChannelRequest request)
    {
      if (request is null)
      {
        return null;
      }

      (bool isSucces, string resizedContent, string extension) = request.Image is null
        ? (false, null, null)
        : (await _resizeHelper.ResizeAsync(request.Image.Content, request.Image.Extension));

      return new DbChannel
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        ImageContent = isSucces ? resizedContent : null,
        ImageExtension = isSucces ? extension : null,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
      };
    }
  }
}
