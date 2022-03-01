using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public record NewsInfo
  {
    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Subject { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
    public List<TagsInfo> Tags { get; set; }
    public ChannelInfo Channel { get; set; }
    public UserInfo Publisher { get; set; }
    public UserInfo Creator { get; set; }
  }
}
