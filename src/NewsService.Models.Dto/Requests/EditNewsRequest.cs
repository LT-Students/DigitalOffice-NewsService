using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Requests
{
  public record EditNewsRequest
  {
    public string Preview { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public string Pseudonym { get; set; }
    public Guid AuthorId { get; set; }
    public Guid? DepartmentId { get; set; }
    public bool IsActive { get; set; }
  }
}
