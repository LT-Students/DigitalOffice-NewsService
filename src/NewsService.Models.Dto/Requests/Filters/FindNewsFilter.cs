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

    [FromQuery(Name = "pseudonym")]
    public string Pseudonym { get; set; }

    [FromQuery(Name = "subject")]
    public string Subject { get; set; }

    [FromQuery(Name = "prewiew")]
    public string Prewiew { get; set; }
  }
}
