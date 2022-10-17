using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.NewsService.Data.Provider;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.NewsService.Models.Db;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.NewsService.Business.Commands.News.Create
{
  public class CreateNewsHandler : IRequestHandler<CreateNewsRequest, Guid?>
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateNewsHandler(
      IDataProvider provider,
      IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid?> Handle(CreateNewsRequest request, CancellationToken token)
    {
      DbNews news = new()
      {
        Id = Guid.NewGuid(),
        Preview = request.Preview,
        Content = request.Content,
        Subject = request.Subject,
        IsActive = request.IsActive,
        ChannelId = request.ChannelId,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        PublishedAtUtc = request.IsActive ? DateTime.UtcNow : null,
        PublishedBy = request.IsActive ? _httpContextAccessor.HttpContext.GetUserId() : null,
      };

      await _provider.News.AddAsync(news, token);
      await _provider.SaveAsync();

      return news.Id;
    }
  }
}
