using System;
using LT.DigitalOffice.Kernel.Validators.Models;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests.Filters
{
  public record FindNewsFilter : BaseFindRequest
  {
    [FromQuery(Name = "authorId")]
    public Guid? AuthorId { get; set; }

    [FromQuery(Name = "departmentId")]
    public Guid? DepartmentId { get; set; }

    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
