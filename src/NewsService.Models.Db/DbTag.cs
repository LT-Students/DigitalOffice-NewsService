using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes.ParseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbTag
  {
    public const string TableName = "Tags";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    [IgnoreParse]
    public ICollection<DbNewsTag> NewsTags { get; set; }

    public DbTag()
    {
      NewsTags = new HashSet<DbNewsTag>();
    }
  }

  public class DbTagConfiguration : IEntityTypeConfiguration<DbTag>
  {
    public void Configure(EntityTypeBuilder<DbTag> builder)
    {
      builder
        .ToTable(DbTag.TableName);

      builder
        .HasKey(p => p.Id);

      builder
        .Property(p => p.Name)
        .IsRequired();

      builder
        .HasMany(p => p.NewsTags)
        .WithOne(p => p.Tag);
    }
  }
}
