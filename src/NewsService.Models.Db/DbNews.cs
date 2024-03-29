﻿using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes.ParseEntity;
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
    public string Subject { get; set; }
    public bool IsActive { get; set; }
    public Guid? ChannelId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? PublishedBy { get; set; }
    public DateTime? PublishedAtUtc { get; set;}
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    [IgnoreParse]
    public ICollection<DbNewsTag> NewsTags { get; set; }
    [IgnoreParse]
    public DbChannel Channel { get; set; }

    public DbNews()
    {
      NewsTags = new HashSet<DbNewsTag>();
    }
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

      builder
        .HasOne(n => n.Channel)
        .WithMany(c => c.News);

      builder
        .HasMany(n => n.NewsTags)
        .WithOne(nt => nt.News);
    }
  }
}
