using System;
using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(NewsServiceDbContext))]
  [Migration("20220304130450_InitialCreate")]
  public class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbNews.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Content = table.Column<string>(nullable: false),
          Subject = table.Column<string>(nullable: false),
          Preview = table.Column<string>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          ChannelId = table.Column<Guid>(nullable: true),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          PublishedAtUtc = table.Column<DateTime>(nullable: true),
          PublishedBy = table.Column<Guid>(nullable: true),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbNews.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
        name: DbChannel.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false, maxLength: 100),
          ImageContent = table.Column<string>(nullable: true),
          ImageExtension = table.Column<string>(nullable: true),
          PinnedMessage = table.Column<string>(nullable: true),
          PinnedNewsId = table.Column<Guid>(nullable: true),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbChannel.TableName}", x => x.Id);
          table.UniqueConstraint("UX_Channel_Name_Unique", x => x.Name);
        });

      migrationBuilder.CreateTable(
        name: DbTag.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          Count = table.Column<int>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey($"PK_{DbTag.TableName}", x => x.Id);
        });

      migrationBuilder.CreateTable(
       name: DbNewsTag.TableName,
       columns: table => new
       {
         Id = table.Column<Guid>(nullable: false),
         NewsId = table.Column<Guid>(nullable: false),
         TagId = table.Column<Guid>(nullable: false),
         CreatedBy = table.Column<Guid>(nullable: false),
         CreatedAtUtc = table.Column<DateTime>(nullable: false),
       },
       constraints: table =>
       {
         table.PrimaryKey($"PK_{DbNewsTag.TableName}", x => x.Id);
       });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
        name: nameof(DbNews));

      migrationBuilder.DropTable(
        name: nameof(DbChannel));

      migrationBuilder.DropTable(
        name: nameof(DbTag));

      migrationBuilder.DropTable(
        name: nameof(DbNewsTag));
    }
  }
}
