using System;
using System.Collections.Generic;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Responses
{
  public class ChannelResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PinnedMessage { get; set; }
    public Guid? PinnedNewsId { get; set; }
    public ImageConsist Image { get; set; }
    public List<NewsInfo> News { get; set; }
  }
}
