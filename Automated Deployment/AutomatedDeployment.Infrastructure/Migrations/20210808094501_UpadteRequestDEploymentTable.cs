using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class UpadteRequestDEploymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Applications_ApplicationId",
                table: "RequestDeploymentHubsApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Hubs_HubId",
                table: "RequestDeploymentHubsApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeploymentHubsApplications_RequestDeployments_RequestDeploymentId",
                table: "RequestDeploymentHubsApplications");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Applications_ApplicationId",
                table: "RequestDeploymentHubsApplications",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Hubs_HubId",
                table: "RequestDeploymentHubsApplications",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "HubID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeploymentHubsApplications_RequestDeployments_RequestDeploymentId",
                table: "RequestDeploymentHubsApplications",
                column: "RequestDeploymentId",
                principalTable: "RequestDeployments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Applications_ApplicationId",
                table: "RequestDeploymentHubsApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Hubs_HubId",
                table: "RequestDeploymentHubsApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDeploymentHubsApplications_RequestDeployments_RequestDeploymentId",
                table: "RequestDeploymentHubsApplications");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Applications_ApplicationId",
                table: "RequestDeploymentHubsApplications",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeploymentHubsApplications_Hubs_HubId",
                table: "RequestDeploymentHubsApplications",
                column: "HubId",
                principalTable: "Hubs",
                principalColumn: "HubID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDeploymentHubsApplications_RequestDeployments_RequestDeploymentId",
                table: "RequestDeploymentHubsApplications",
                column: "RequestDeploymentId",
                principalTable: "RequestDeployments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
