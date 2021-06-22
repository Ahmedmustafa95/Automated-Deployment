using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class AddManyToManyBetweenDeploymentAndDeploymentDetailsAndHubApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DeploymentDetails_HubId_AppId",
                table: "DeploymentDetails",
                columns: new[] { "HubId", "AppId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentDetails_HubsApplications_HubId_AppId",
                table: "DeploymentDetails",
                columns: new[] { "HubId", "AppId" },
                principalTable: "HubsApplications",
                principalColumns: new[] { "HubID", "AppID" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentDetails_HubsApplications_HubId_AppId",
                table: "DeploymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_DeploymentDetails_HubId_AppId",
                table: "DeploymentDetails");
        }
    }
}
