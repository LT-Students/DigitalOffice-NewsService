using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public class FindNewsFilter
  {
    [FromQuery(Name = "skipCount")]
    public int SkipCount { get; set; }

    [FromQuery(Name = "takeCount")]
    public int TakeCount { get; set; }

    [FromQuery(Name = "authorId")]
    public Guid? AuthorId { get; set; }

    [FromQuery(Name = "departmentId")]
    public Guid? DepartmentId { get; set; }

    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
