using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Configurations
{
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
    }
  }
}
