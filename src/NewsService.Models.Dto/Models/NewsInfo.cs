using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public record NewsInfo
  {
    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Subject { get; set; }
    public User Author { get; set; }
    public Department Department { get; set; }
    public bool IsActive { get; set; }
  }
}
