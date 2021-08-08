using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class UpadteRequestDEploymentTable34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RollbackScript",
                table: "RequestDeployments",
                newName: "SignOff");

            migrationBuilder.AddColumn<string>(
                name: "RollbackPlan",
                table: "RequestDeployments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollbackPlan",
                table: "RequestDeployments");

            migrationBuilder.RenameColumn(
                name: "SignOff",
                table: "RequestDeployments",
                newName: "RollbackScript");
        }
    }
}
