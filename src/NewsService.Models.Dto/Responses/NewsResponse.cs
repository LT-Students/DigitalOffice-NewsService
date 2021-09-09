using System;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Responses
{
  public record NewsResponse
  {
    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public User Author { get; set; }
    public Department Department { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
}
