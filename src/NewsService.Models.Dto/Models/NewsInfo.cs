using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public record NewsInfo
  {
    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Subject { get; set; }
    public List<UserInfo> Authors { get; set; }
    public List<DepartmentInfo> Departments { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
}
