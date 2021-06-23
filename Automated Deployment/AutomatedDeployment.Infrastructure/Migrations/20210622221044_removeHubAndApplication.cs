using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class removeHubAndApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_Applications_ApplicationAppID",
                table: "Deployments");

            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_Hubs_HubID",
                table: "Deployments");

            migrationBuilder.DropIndex(
                name: "IX_Deployments_ApplicationAppID",
                table: "Deployments");

            migrationBuilder.DropIndex(
                name: "IX_Deployments_HubID",
                table: "Deployments");

            migrationBuilder.DropColumn(
                name: "ApplicationAppID",
                table: "Deployments");

            migrationBuilder.DropColumn(
                name: "HubID",
                table: "Deployments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationAppID",
                table: "Deployments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HubID",
                table: "Deployments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_ApplicationAppID",
                table: "Deployments",
                column: "ApplicationAppID");

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_HubID",
                table: "Deployments",
                column: "HubID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_Applications_ApplicationAppID",
                table: "Deployments",
                column: "ApplicationAppID",
                principalTable: "Applications",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_Hubs_HubID",
                table: "Deployments",
                column: "HubID",
                principalTable: "Hubs",
                principalColumn: "HubID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
