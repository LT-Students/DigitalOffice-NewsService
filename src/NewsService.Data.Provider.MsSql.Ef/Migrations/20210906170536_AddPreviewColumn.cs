using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(NewsServiceDbContext))]
  [Migration("20210906170536_AddPreviewColumn")]
  public class AddPreviewColumn : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        name: nameof(DbNews.Preview),
        table: DbNews.TableName,
        nullable: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        name: "Preview",
        table: DbNews.TableName);
    }
  }
}
