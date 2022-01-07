using Microsoft.EntityFrameworkCore.Migrations;

namespace FilesManager.Infrastructure.Migrations
{
    public partial class Changed_FileMetadata_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "FileMetadata",
                newName: "RemoteId");

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "FileMetadata",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "FileMetadata");

            migrationBuilder.RenameColumn(
                name: "RemoteId",
                table: "FileMetadata",
                newName: "Extension");
        }
    }
}
