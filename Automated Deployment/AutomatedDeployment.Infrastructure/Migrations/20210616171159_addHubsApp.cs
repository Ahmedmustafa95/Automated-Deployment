using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class addHubsApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.CreateTable(
                name: "HubsApplications",
                columns: table => new
                {
                    HubID = table.Column<int>(type: "int", nullable: false),
                    AppID = table.Column<int>(type: "int", nullable: false),
                    AssemblyPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubsApplications", x => new { x.HubID, x.AppID });
                    table.ForeignKey(
                        name: "FK_HubsApplications_Applications_AppID",
                        column: x => x.AppID,
                        principalTable: "Applications",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HubsApplications_Hubs_HubID",
                        column: x => x.HubID,
                        principalTable: "Hubs",
                        principalColumn: "HubID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubsApplications_AppID",
                table: "HubsApplications",
                column: "AppID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubsApplications");

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    HubID = table.Column<int>(type: "int", nullable: false),
                    AppID = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssemblyPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigurationPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeployedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequestedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
    }
}
