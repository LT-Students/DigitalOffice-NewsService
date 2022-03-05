using System;
using System.Collections.Generic;
using LT.DigitalOffice.Kernel.BrokerSupport.Attributes.ParseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.NewsService.Models.Db
{
  public class DbChannel
  {
    public const string TableName = "Channels";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PinnedMessage { get; set; }
    public Guid? PinnedNewsId { get; set; }
    public string ImageContent { get; set; }
    public string ImageExtension { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    [IgnoreParse]
    public ICollection<DbNews> News { get; set; }

    public DbChannel()
    {
      News = new HashSet<DbNews>();
    }
  }

  public class DbChanelConfiguration : IEntityTypeConfiguration<DbChannel>
  {
    public void Configure(EntityTypeBuilder<DbChannel> builder)
    {
      builder.
        ToTable(DbChannel.TableName);

      builder.
        HasKey(p => p.Id);

      builder
        .Property(p => p.Name)
        .IsRequired();

      builder
        .HasMany(p => p.News)
        .WithOne(n => n.Channel);
    }
  }
}
