﻿using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public record FindChannelFilter : BaseFindFilter
  {
    [FromQuery(Name = "name")]
    public string Name { get; set; }

    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
