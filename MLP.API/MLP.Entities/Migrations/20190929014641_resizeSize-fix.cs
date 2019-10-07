using Microsoft.EntityFrameworkCore.Migrations;

namespace MLP.API.Migrations
{
    public partial class resizeSizefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resizeSize",
                table: "ImageProcessingConfigs",
                newName: "ResizeSize");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResizeSize",
                table: "ImageProcessingConfigs",
                newName: "resizeSize");
        }
    }
}
