using System;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes.ParseEntity;
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

    [IgnoreParse]
    public DbNews News { get; set; }
    [IgnoreParse]
    public DbTag Tag { get; set; }
  }

  public class DbNewsTagConfiguration : IEntityTypeConfiguration<DbNewsTag>
  {
    public void Configure(EntityTypeBuilder<DbNewsTag> builder)
    {
      builder
        .ToTable(DbNewsTag.TableName);

      builder
        .HasKey(p => p.Id);

      builder
        .HasOne(nt => nt.News)
        .WithMany(n => n.NewsTags);

      builder
        .HasOne(nt => nt.Tag)
        .WithMany(n => n.NewsTags);
    }
  }
}
