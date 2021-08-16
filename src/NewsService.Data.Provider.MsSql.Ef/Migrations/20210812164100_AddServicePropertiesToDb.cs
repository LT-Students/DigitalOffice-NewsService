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
                table: "News",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: nameof(DbNews.CreatedAtUtc),
                table: "News",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: nameof(DbNews.ModifiedBy),
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: nameof(DbNews.ModifiedAtUtc),
                table: "News",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "News");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "News");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                table: "News");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "News");
        }
    }
}
