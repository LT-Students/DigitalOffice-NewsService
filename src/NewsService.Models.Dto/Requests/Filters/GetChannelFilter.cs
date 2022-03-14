using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public record GetChannelFilter : BaseFindFilter
  {
    [FromQuery(Name = "includeNews")]
    public bool IncludeNews { get; set; } = false;
  }
}
