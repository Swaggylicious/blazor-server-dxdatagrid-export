using Microsoft.EntityFrameworkCore.Migrations;

namespace DxDataGridExportingWithReports.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpDesciption = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpSql = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpParamModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seq = table.Column<int>(type: "int", nullable: false),
                    ParamName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParamType = table.Column<int>(type: "int", nullable: false),
                    ParamValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpParamModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpParamModels_SpModels_SpModelId",
                        column: x => x.SpModelId,
                        principalTable: "SpModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpParamModels_SpModelId",
                table: "SpParamModels",
                column: "SpModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpParamModels");

            migrationBuilder.DropTable(
                name: "SpModels");
        }
    }
}
