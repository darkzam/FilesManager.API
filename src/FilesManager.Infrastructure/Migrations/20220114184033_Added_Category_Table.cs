using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilesManager.Infrastructure.Migrations
{
    public partial class Added_Category_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "FileMetadata",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileMetadata_CategoryId",
                table: "FileMetadata",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileMetadata_Category_CategoryId",
                table: "FileMetadata",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileMetadata_Category_CategoryId",
                table: "FileMetadata");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_FileMetadata_CategoryId",
                table: "FileMetadata");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FileMetadata");
        }
    }
}
