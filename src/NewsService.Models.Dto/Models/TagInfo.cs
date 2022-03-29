using System;

namespace LT.DigitalOffice.NewsService.Models.Dto.Models
{
  public record TagInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
  }
}
