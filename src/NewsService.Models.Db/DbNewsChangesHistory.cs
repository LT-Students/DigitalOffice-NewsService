using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbNewsChangesHistory
    {
        public const string TableName = "NewsChangesHistory";

        public Guid Id { get; set; }
        public Guid NewsId { get; set; }
        public Guid ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        public DbNews News { get; set; }
        public ICollection<DbChangeSetDetails> ChangeSetDetails { get; set; }

        public DbNewsChangesHistory()
        {
            ChangeSetDetails = new HashSet<DbChangeSetDetails>();
        }
    }

    public class DbNewsHistoryConfiguration : IEntityTypeConfiguration<DbNewsChangesHistory>
    {
        public void Configure(EntityTypeBuilder<DbNewsChangesHistory> builder)
        {
            builder
               .ToTable(DbNewsChangesHistory.TableName);

            builder.HasKey(nh => nh.Id);

            builder
                .HasOne(nh => nh.News)
                .WithMany(n => n.NewsHistory)
                .HasForeignKey(nh => nh.NewsId);

            builder
                .HasMany(nh => nh.ChangeSetDetails)
                .WithOne(cs => cs.NewsHistory);
        }
    }
}
