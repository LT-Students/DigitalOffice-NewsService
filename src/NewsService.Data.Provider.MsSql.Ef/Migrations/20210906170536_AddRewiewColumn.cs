using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(NewsServiceDbContext))]
  [Migration("20210906170536_AddPrewiewAndDropSenderIdColumn")]
  public class AddPrewiewColumn : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        name: nameof(DbNews.Prewiew),
        table: DbNews.TableName,
        nullable: true);
    }
  }
}
