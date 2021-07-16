using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false),
                    AuthorName = table.Column<string>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
