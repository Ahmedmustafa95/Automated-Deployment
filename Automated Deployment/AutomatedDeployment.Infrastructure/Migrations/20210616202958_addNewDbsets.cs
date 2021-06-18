using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class addNewDbsets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployment_Applications_AppID",
                table: "Deployment");

            migrationBuilder.DropForeignKey(
                name: "FK_Deployment_Hubs_HubID",
                table: "Deployment");

            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentFiles_Deployment_DeploymentID",
                table: "DeploymentFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deployment",
                table: "Deployment");

            migrationBuilder.RenameTable(
                name: "Deployment",
                newName: "Deployments");

            migrationBuilder.RenameIndex(
                name: "IX_Deployment_HubID",
                table: "Deployments",
                newName: "IX_Deployments_HubID");

            migrationBuilder.RenameIndex(
                name: "IX_Deployment_AppID",
                table: "Deployments",
                newName: "IX_Deployments_AppID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deployments",
                table: "Deployments",
                column: "DeploymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentFiles_Deployments_DeploymentID",
                table: "DeploymentFiles",
                column: "DeploymentID",
                principalTable: "Deployments",
                principalColumn: "DeploymentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_Applications_AppID",
                table: "Deployments",
                column: "AppID",
                principalTable: "Applications",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_Hubs_HubID",
                table: "Deployments",
                column: "HubID",
                principalTable: "Hubs",
                principalColumn: "HubID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentFiles_Deployments_DeploymentID",
                table: "DeploymentFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_Applications_AppID",
                table: "Deployments");

            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_Hubs_HubID",
                table: "Deployments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deployments",
                table: "Deployments");

            migrationBuilder.RenameTable(
                name: "Deployments",
                newName: "Deployment");

            migrationBuilder.RenameIndex(
                name: "IX_Deployments_HubID",
                table: "Deployment",
                newName: "IX_Deployment_HubID");

            migrationBuilder.RenameIndex(
                name: "IX_Deployments_AppID",
                table: "Deployment",
                newName: "IX_Deployment_AppID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deployment",
                table: "Deployment",
                column: "DeploymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployment_Applications_AppID",
                table: "Deployment",
                column: "AppID",
                principalTable: "Applications",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deployment_Hubs_HubID",
                table: "Deployment",
                column: "HubID",
                principalTable: "Hubs",
                principalColumn: "HubID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentFiles_Deployment_DeploymentID",
                table: "DeploymentFiles",
                column: "DeploymentID",
                principalTable: "Deployment",
                principalColumn: "DeploymentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
