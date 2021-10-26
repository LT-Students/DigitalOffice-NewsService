using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(NewsServiceDbContext))]
  [Migration("20211025122100_RemoveDepartmentIdColumn")]
  public class RemoveDepartmentIdColumn : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.DropColumn(
        name: "DepartmentId",
        table: DbNews.TableName);
    }
  }
}
