using System;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel
{
  public record CreateChannelRequest
  {
    public string Name { get; set; }
    public string PinnedMessage { get; set; }
    public Guid? PinnedNewsId { get; set; }
    public ImageConsist Image { get; set; }
  }
}
