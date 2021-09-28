using System;
using System.Collections.Generic;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests
{
  public record CreateNewsRequest
  {
    public string Preview { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public string Pseudonym { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? DepartmentId { get; set; }
  }
}
