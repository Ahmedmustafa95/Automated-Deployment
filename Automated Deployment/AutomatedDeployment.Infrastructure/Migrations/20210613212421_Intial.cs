using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class Intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    AppID = table.Column<int>(type: "int", nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.AppID);
                });

            migrationBuilder.CreateTable(
                name: "Hubs",
                columns: table => new
                {
                    HubID = table.Column<int>(type: "int", nullable: false),
                    HubName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hubs", x => x.HubID);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    HubID = table.Column<int>(type: "int", nullable: false),
                    AppID = table.Column<int>(type: "int", nullable: false),
                    AssemblyPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigurationPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeployedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequestedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => new { x.HubID, x.AppID });
                    table.ForeignKey(
                        name: "FK_Configurations_Applications",
                        column: x => x.AppID,
                        principalTable: "Applications",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configurations_Hubs",
                        column: x => x.HubID,
                        principalTable: "Hubs",
                        principalColumn: "HubID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configurations_AppID",
                table: "Configurations",
                column: "AppID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Hubs");
        }
    }
}
