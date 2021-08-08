using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    AppID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.AppID);
                });

            migrationBuilder.CreateTable(
                name: "Deployments",
                columns: table => new
                {
                    DeploymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeploymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeploymentType = table.Column<int>(type: "int", nullable: false),
                    OriginalDeployment = table.Column<int>(type: "int", nullable: true),
                    DeployedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.DeploymentID);
                });

            migrationBuilder.CreateTable(
                name: "Hubs",
                columns: table => new
                {
                    HubID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HubName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hubs", x => x.HubID);
                });

            migrationBuilder.CreateTable(
                name: "HubsApplications",
                columns: table => new
                {
                    HubID = table.Column<int>(type: "int", nullable: false),
                    AppID = table.Column<int>(type: "int", nullable: false),
                    AssemblyPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigFilepPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "DeploymentDetails",
                columns: table => new
                {
                    DeploymentDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HubId = table.Column<int>(type: "int", nullable: false),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    DeploymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentDetails", x => x.DeploymentDetailsId);
                    table.ForeignKey(
                        name: "FK_DeploymentDetails_Deployments_DeploymentId",
                        column: x => x.DeploymentId,
                        principalTable: "Deployments",
                        principalColumn: "DeploymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeploymentDetails_HubsApplications_HubId_AppId",
                        columns: x => new { x.HubId, x.AppId },
                        principalTable: "HubsApplications",
                        principalColumns: new[] { "HubID", "AppID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeploymentFiles",
                columns: table => new
                {
                    DeploymentFilesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeploymentDetailsId = table.Column<int>(type: "int", nullable: false),
                    FilesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentFiles", x => x.DeploymentFilesID);
                    table.ForeignKey(
                        name: "FK_DeploymentFiles_DeploymentDetails_DeploymentDetailsId",
                        column: x => x.DeploymentDetailsId,
                        principalTable: "DeploymentDetails",
                        principalColumn: "DeploymentDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentDetails_DeploymentId",
                table: "DeploymentDetails",
                column: "DeploymentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentDetails_HubId_AppId",
                table: "DeploymentDetails",
                columns: new[] { "HubId", "AppId" });

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentFiles_DeploymentDetailsId",
                table: "DeploymentFiles",
                column: "DeploymentDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_HubsApplications_AppID",
                table: "HubsApplications",
                column: "AppID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeploymentFiles");

            migrationBuilder.DropTable(
                name: "DeploymentDetails");

            migrationBuilder.DropTable(
                name: "Deployments");

            migrationBuilder.DropTable(
                name: "HubsApplications");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Hubs");
        }
    }
}
