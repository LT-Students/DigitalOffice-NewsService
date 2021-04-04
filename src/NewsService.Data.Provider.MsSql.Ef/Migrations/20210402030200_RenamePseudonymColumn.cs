using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(NewsServiceDbContext))]
    [Migration("20210402030200_RenamePseudonymColumn")]
    public class RenamePseudonymColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "News",
                newName: "Pseudonym");

            migrationBuilder.AlterColumn<string>(
                name: "Pseudonym",
                table: "News",
                nullable: true);
        }
    }
}
