using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbNews
    {
        public const string TableName = "News";

        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DbNewsChangesHistory> NewsHistory { get; set; }

        public DbNews()
        {
            NewsHistory = new HashSet<DbNewsChangesHistory>();
        }
    }

    public class DbProjectConfiguration : IEntityTypeConfiguration<DbNews>
    {
        public void Configure(EntityTypeBuilder<DbNews> builder)
        {
            builder
            .ToTable(DbNews.TableName);

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Content)
                .IsRequired();

            builder
                .Property(p => p.Subject)
                .IsRequired();

            builder
                .Property(p => p.AuthorName)
                .IsRequired();

            builder
                .HasMany(n => n.NewsHistory)
                .WithOne(nh => nh.News);
        }
    }
}