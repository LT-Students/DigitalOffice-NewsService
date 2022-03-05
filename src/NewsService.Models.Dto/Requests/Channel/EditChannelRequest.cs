using System;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Channel
{
  public class EditChannelRequest
  {
    public string PinnedMessage { get; set; }
    public Guid? PinnedNewsId { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public ImageConsist Image { get; set; }
  }
}
