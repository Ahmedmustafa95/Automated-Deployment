using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class adddeploymnet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deployment",
                columns: table => new
                {
                    DeploymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HubID = table.Column<int>(type: "int", nullable: false),
                    AppID = table.Column<int>(type: "int", nullable: false),
                    DeploymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployment", x => x.DeploymentID);
                    table.ForeignKey(
                        name: "FK_Deployment_Applications_AppID",
                        column: x => x.AppID,
                        principalTable: "Applications",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deployment_Hubs_HubID",
                        column: x => x.HubID,
                        principalTable: "Hubs",
                        principalColumn: "HubID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeploymentFiles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeploymentID = table.Column<int>(type: "int", nullable: false),
                    FilesName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeploymentFiles_Deployment_DeploymentID",
                        column: x => x.DeploymentID,
                        principalTable: "Deployment",
                        principalColumn: "DeploymentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deployment_AppID",
                table: "Deployment",
                column: "AppID");

            migrationBuilder.CreateIndex(
                name: "IX_Deployment_HubID",
                table: "Deployment",
                column: "HubID");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentFiles_DeploymentID",
                table: "DeploymentFiles",
                column: "DeploymentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeploymentFiles");

            migrationBuilder.DropTable(
                name: "Deployment");
        }
    }
}
