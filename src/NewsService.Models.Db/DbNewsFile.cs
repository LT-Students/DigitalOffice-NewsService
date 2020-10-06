using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbNewsFile
    {
        public Guid NewsId { get; set; }
        public Guid FileId { get; set; }
        public DbNews News { get; set; }
    }
    public class NewsFileConfiguration : IEntityTypeConfiguration<DbNewsFile>
    {
        public void Configure(EntityTypeBuilder<DbNewsFile> builder)
        {
            builder.HasKey(news => new { news.NewsId, news.FileId });

            builder.HasOne(news => news.News)
                .WithMany(news => news.FileIds)
                .HasForeignKey(news => news.NewsId);
        }
    }
}
