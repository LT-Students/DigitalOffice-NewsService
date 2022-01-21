using System;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.NewsService.Models.Dto.Configuration;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbNews
  {
    public const string TableName = "News";

    public Guid Id { get; set; }

    public string Preview { get; set; }

    public string Content { get; set; }

    [Keyword((int)ServiceEndpoints.CreateNews, (int)ServiceEndpoints.EditNews)]
    public string Subject { get; set; }

    public string Pseudonym { get; set; }

    public Guid AuthorId { get; set; }

    [Keyword((int)ServiceEndpoints.EditNews)]
    public bool IsActive { get; set; }

    [Keyword((int)ServiceEndpoints.CreateNews)]
    public Guid CreatedBy { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    [Keyword((int)ServiceEndpoints.EditNews)]
    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedAtUtc { get; set; }
  }
}
