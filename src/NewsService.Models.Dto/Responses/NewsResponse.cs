using System;
using LT.DigitalOffice.NewsService.Models.Dto.Models;

namespace LT.DigitalOffice.NewsService.Models.Dto.Responses
{
  public class NewsResponse
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public User Author { get; set; }
    public Department Department { get; set; }
    public bool IsActive { get; set; }
    public string Prewiew { get; set; }
  }
}
