using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbNews
  {
    public const string TableName = "News";

    public Guid Id { get; set; }
    public string Preview { get; set; }
    public string Content { get; set; }
    [KeyWord]
    public string Subject { get; set; }
    public string Pseudonym { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    [KeyWord]
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
  }

  public class DbNewsConfiguration : IEntityTypeConfiguration<DbNews>
  {
    public void Configure(EntityTypeBuilder<DbNews> builder)
    {
      builder.
        ToTable(DbNews.TableName);

      builder.
        HasKey(p => p.Id);

      builder
        .Property(p => p.Content)
        .IsRequired();

      builder
        .Property(p => p.Subject)
        .IsRequired();

      builder
        .Property(p => p.IsActive)
        .IsRequired();
    }
  }
}
