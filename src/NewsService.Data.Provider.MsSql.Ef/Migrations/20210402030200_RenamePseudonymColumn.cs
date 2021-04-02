using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(RenamePseudonymColumn))]
    [Migration("20210402030200_RenamePseudonymColumn")]
    public class RenamePseudonymColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "Pseudonym",
               table: "News",
               nullable: true);

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "News");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pseudonym",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "News",
                nullable: true);
        }
    }
}
