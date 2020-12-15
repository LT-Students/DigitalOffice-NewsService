using LT.DigitalOffice.NewsService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.NewsService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(NewsServiceDbContext))]
    [Migration("20201214161841_AddNewsHistory")]
    public partial class AddNewsHistory : Migration
    {
        private const string ColumnIdName = "Id";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreateNewsChangesHistoryTable(migrationBuilder);
            CreateChangeSetDetailsTable(migrationBuilder);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeSetDetails_NewsHistoryId",
                table: DbChangeSetDetails.TableName,
                column: "NewsHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsChangesHistory_NewsId",
                table: DbNewsChangesHistory.TableName,
                column: "NewsId");
        }

        #region CreateTables

        private void CreateChangeSetDetailsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbChangeSetDetails.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NewsHistoryId = table.Column<Guid>(nullable: false),
                    FieldName = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeSetDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeSetDetails_NewsChangesHistory_NewsHistoryId",
                        column: x => x.NewsHistoryId,
                        principalTable: DbNewsChangesHistory.TableName,
                        principalColumn: ColumnIdName,
                        onDelete: ReferentialAction.Cascade);
                });
        }

        private void CreateNewsChangesHistoryTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbNewsChangesHistory.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NewsId = table.Column<Guid>(nullable: false),
                    ChangedBy = table.Column<DateTime>(nullable: false),
                    ChangadAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsChangesHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsChangesHistory_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: DbNews.TableName,
                        principalColumn: ColumnIdName,
                        onDelete: ReferentialAction.Cascade);
                });
        }
        #endregion

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: DbChangeSetDetails.TableName);

            migrationBuilder.DropTable(
                name: DbNewsChangesHistory.TableName);
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");
        }
    }
}
