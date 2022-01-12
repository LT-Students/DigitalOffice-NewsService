using System;
using LT.DigitalOffice.NewsService.Models.Dto.Configuration;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbNews
  {
    public const string TableName = "News";

    public Guid Id { get; set; }

    public string Preview { get; set; }

    public string Content { get; set; }

    [KeyWord(ServiceEndpoints.CreateNews, ServiceEndpoints.EditNews)]
    public string Subject { get; set; }

    public string Pseudonym { get; set; }

    public Guid AuthorId { get; set; }

    [KeyWord(ServiceEndpoints.EditNews)]
    public bool IsActive { get; set; }

    [KeyWord(ServiceEndpoints.CreateNews)]
    public Guid CreatedBy { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    [KeyWord(ServiceEndpoints.EditNews)]
    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedAtUtc { get; set; }
  }
}
