using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public class ChannelInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PinnedMessage { get; set; }
    public Guid? PinnedNewsId { get; set; }
    public ImageConsist Image { get; set; }
  }
}
