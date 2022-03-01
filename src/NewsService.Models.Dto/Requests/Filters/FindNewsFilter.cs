using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public record FindNewsFilter : BaseFindFilter
  {
    [FromQuery(Name = "creatorId")]
    public Guid? Creator { get; set; }

    [FromQuery(Name = "publisherId")]
    public Guid? Publisher { get; set; }

    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
