using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.News
{
  public record CreateNewsRequest
  {
    public string Preview { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public Guid? ChannelId { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> TagsIds { get; set; }
  }
}
