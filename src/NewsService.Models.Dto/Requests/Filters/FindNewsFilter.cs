using System;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public record FindNewsFilter : BaseFindFilter
  {
    [FromQuery(Name = "publisherId")]
    public Guid? PublisherId { get; set; }

    [FromQuery(Name = "isActive")]
    public bool? IsActive { get; set; }

    [FromQuery(Name = "includeChannel")]
    public bool IncludeChannel { get; set; } = false;

    [FromQuery(Name = "channelId")]
    public Guid? ChannelId { get; set; }

    [FromQuery(Name = "tagId")]
    public Guid? TagId { get; set; }
  }
}
