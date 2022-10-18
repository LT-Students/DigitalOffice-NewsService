using System;
using MediatR;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Business.Commands.News.Create
{
  public record CreateNewsRequest : IRequest<Guid?>
  {
    public string Preview { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public Guid? ChannelId { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> TagsIds { get; set; }
  }
}
