using System;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public class FindNewsFilter
  {
    [FromQuery(Name = "authorid")]
    public Guid? AuthorId { get; set; }

    [FromQuery(Name = "departmentid")]
    public Guid? DepartmentId { get; set; }

    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
