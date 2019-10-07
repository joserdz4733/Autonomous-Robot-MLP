using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLP.API.Migrations
{
    public partial class mlp20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageProcessingConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NeuralNetworkId = table.Column<Guid>(nullable: false),
                    ValuesFactor = table.Column<double>(nullable: false),
                    ConfigName = table.Column<string>(nullable: false),
                    BlueAvg = table.Column<double>(nullable: false),
                    BlueStd = table.Column<double>(nullable: false),
                    GreenAvg = table.Column<double>(nullable: false),
                    GreenStd = table.Column<double>(nullable: false),
                    RedAvg = table.Column<double>(nullable: false),
                    RedStd = table.Column<double>(nullable: false),
                    ImageFilter = table.Column<int>(nullable: false),
                    ImageFilterSize = table.Column<int>(nullable: false),
                    resizeSize = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageProcessingConfigs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageProcessingConfigs");
        }
    }
}
