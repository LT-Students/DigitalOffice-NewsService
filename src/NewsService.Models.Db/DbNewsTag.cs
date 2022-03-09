using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbNewsTag
  {
    public const string TableName = "NewsTags";

    public Guid Id { get; set; }
    public Guid NewsId { get; set; }
    public Guid TagId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }

  public class DbNewsTagConfiguration : IEntityTypeConfiguration<DbNewsTag>
  {
    public void Configure(EntityTypeBuilder<DbNewsTag> builder)
    {
      builder
        .ToTable(DbNewsTag.TableName);

      builder
        .HasKey(p => p.Id);
    }
  }
}
