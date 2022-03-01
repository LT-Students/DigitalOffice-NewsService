using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public record FullNewsInfo
  {
    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
    public List<TagsInfo> Tags { get; set; }
    public ChannelInfo Chanel { get; set; }
    public UserInfo Publisher { get; set; }
    public UserInfo Creator { get; set; }
  }
}
