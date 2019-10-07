using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLP.API.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NeuralNetworks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeuralNetworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NeuralNetworkdTrainingConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NeuralNetworkId = table.Column<Guid>(nullable: false),
                    Epochs = table.Column<int>(nullable: false),
                    Eta = table.Column<double>(nullable: false),
                    WeightsFactor = table.Column<double>(nullable: false),
                    HiddenNeuronElements = table.Column<int>(nullable: false),
                    HiddenActivationFunction = table.Column<int>(nullable: false),
                    OutputNeuronElements = table.Column<int>(nullable: false),
                    OutputActivationFunction = table.Column<int>(nullable: false),
                    InputSize = table.Column<int>(nullable: false),
                    TrainingDatabaseFileRoute = table.Column<string>(nullable: false),
                    TrainingDatabaseFileName = table.Column<string>(nullable: false),
                    TrainingDatabaseTestFileName = table.Column<string>(nullable: false),
                    TrainingDatabaseType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeuralNetworkdTrainingConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NeuralNetworkdTrainingConfigs_NeuralNetworks_NeuralNetworkId",
                        column: x => x.NeuralNetworkId,
                        principalTable: "NeuralNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Neurons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Index = table.Column<int>(nullable: false),
                    Bias = table.Column<double>(nullable: false),
                    NeuronType = table.Column<int>(nullable: false),
                    NeuralNetworkId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neurons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Neurons_NeuralNetworks_NeuralNetworkId",
                        column: x => x.NeuralNetworkId,
                        principalTable: "NeuralNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredictedObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Index = table.Column<int>(nullable: false),
                    ObjectName = table.Column<string>(nullable: false),
                    NeuralNetworkTrainingConfigId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictedObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredictedObjects_NeuralNetworkdTrainingConfigs_NeuralNetworkTrainingConfigId",
                        column: x => x.NeuralNetworkTrainingConfigId,
                        principalTable: "NeuralNetworkdTrainingConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NeuronWeights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Index = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    NeuronId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeuronWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NeuronWeights_Neurons_NeuronId",
                        column: x => x.NeuronId,
                        principalTable: "Neurons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NeuralNetworkdTrainingConfigs_NeuralNetworkId",
                table: "NeuralNetworkdTrainingConfigs",
                column: "NeuralNetworkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Neurons_NeuralNetworkId",
                table: "Neurons",
                column: "NeuralNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_NeuronWeights_NeuronId",
                table: "NeuronWeights",
                column: "NeuronId");

            migrationBuilder.CreateIndex(
                name: "IX_PredictedObjects_NeuralNetworkTrainingConfigId",
                table: "PredictedObjects",
                column: "NeuralNetworkTrainingConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NeuronWeights");

            migrationBuilder.DropTable(
                name: "PredictedObjects");

            migrationBuilder.DropTable(
                name: "Neurons");

            migrationBuilder.DropTable(
                name: "NeuralNetworkdTrainingConfigs");

            migrationBuilder.DropTable(
                name: "NeuralNetworks");
        }
    }
}
