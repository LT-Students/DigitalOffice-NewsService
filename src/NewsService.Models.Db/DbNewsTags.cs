using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbNewsTags
  {
    public const string TableName = "NewsTags";

    public Guid Id { get; set; }
    public Guid NewsId { get; set; }
    public Guid TagId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }

  public class DbNewsTagsConfiguration : IEntityTypeConfiguration<DbNewsTags>
  {
    public void Configure(EntityTypeBuilder<DbNewsTags> builder)
    {
      builder
        .ToTable(DbNewsTags.TableName);

      builder
        .HasKey(p => p.Id);
    }
  }
}
