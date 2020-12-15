using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbChangeSetDetails
    {
        public const string TableName = "ChangeSetDetails";

        public Guid Id { get; set; }
        public Guid NewsHistoryId { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }

        public DbNewsChangesHistory NewsHistory { get; set; }
    }

    public class DbChangeSetDetailsConfiguration : IEntityTypeConfiguration<DbChangeSetDetails>
    {
        public void Configure(EntityTypeBuilder<DbChangeSetDetails> builder)
        {
            builder
                .ToTable(DbChangeSetDetails.TableName);

            builder.HasKey(cs => cs.Id);

            builder.Property(cs => cs.FieldName);

            builder.Property(cs => cs.Value);

            builder
                .HasOne(cs => cs.NewsHistory)
                .WithMany(n => n.ChangeSetDetails)
                .HasForeignKey(nh => nh.NewsHistoryId);
        }
    }
}
