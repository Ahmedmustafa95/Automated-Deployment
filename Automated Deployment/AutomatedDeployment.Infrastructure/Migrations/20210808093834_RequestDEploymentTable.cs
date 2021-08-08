using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class RequestDEploymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestDeployments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollbackScript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeploymentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDeployments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestDeploymentHubsApplications",
                columns: table => new
                {
                    HubId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    RequestDeploymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDeploymentHubsApplications", x => new { x.ApplicationId, x.HubId, x.RequestDeploymentId });
                    table.ForeignKey(
                        name: "FK_RequestDeploymentHubsApplications_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestDeploymentHubsApplications_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "HubID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestDeploymentHubsApplications_RequestDeployments_RequestDeploymentId",
                        column: x => x.RequestDeploymentId,
                        principalTable: "RequestDeployments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestDeploymentHubsApplications_HubId",
                table: "RequestDeploymentHubsApplications",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDeploymentHubsApplications_RequestDeploymentId",
                table: "RequestDeploymentHubsApplications",
                column: "RequestDeploymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestDeploymentHubsApplications");

            migrationBuilder.DropTable(
                name: "RequestDeployments");
        }
    }
}
