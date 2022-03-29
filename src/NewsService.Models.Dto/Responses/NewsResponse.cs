using System;
using System.Collections.Generic;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Responses
{
  public record NewsResponse
  {
    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public UserInfo Creator { get; set; }
    public UserInfo Publisher { get; set; }
    public ChannelInfo Channel { get; set; }
    public List<TagInfo> Tags { get; set; }
    public bool IsActive { get; set; }
    public DateTime? PublishedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
}
