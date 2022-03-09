using System;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public record FindNewsFilter : BaseFindFilter
  {
    [FromQuery(Name = "publisherId")]
    public Guid? Publisher { get; set; }

    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;

    [FromQuery(Name = "includeChannel")]
    public bool IncludeChannel { get; set; } = false;

    [FromQuery(Name = "channelId")]
    public Guid? ChannelId { get; set; }
  }
}
