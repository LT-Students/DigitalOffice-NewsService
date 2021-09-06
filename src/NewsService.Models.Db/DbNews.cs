using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.NewsService.Models.Db
{
    public class DbNews
    {
        public const string TableName = "News";

        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string Pseudonym { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public string Prewiew { get; set; }
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
                .Property(p => p.AuthorId)
                .IsRequired();

            builder
                .Property(p => p.IsActive)
                .IsRequired();
        }
    }
}
