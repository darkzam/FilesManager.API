using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilesManager.Infrastructure.Migrations
{
    public partial class Added_Tag_Table_And_FileMetadataTag_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileMetadataTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileMetadataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMetadataTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileMetadataTags_FileMetadata_FileMetadataId",
                        column: x => x.FileMetadataId,
                        principalTable: "FileMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileMetadataTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileMetadataTags_FileMetadataId",
                table: "FileMetadataTags",
                column: "FileMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMetadataTags_TagId",
                table: "FileMetadataTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileMetadataTags");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
