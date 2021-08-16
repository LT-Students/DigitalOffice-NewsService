using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(NewsServiceDbContext))]
    [Migration("20210812164100_AddServiceProperties")]
    public class AddServiceProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: nameof(DbNews.CreatedBy),
                table: DbNews.TableName,
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: nameof(DbNews.CreatedAtUtc),
                table: DbNews.TableName,
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: nameof(DbNews.ModifiedBy),
                table: DbNews.TableName,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: nameof(DbNews.ModifiedAtUtc),
                table: DbNews.TableName,
                nullable: true);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: DbNews.TableName);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: DbNews.TableName);

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: DbNews.TableName);

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: DbNews.TableName);

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: DbNews.TableName);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: DbNews.TableName);
        }
    }
}
