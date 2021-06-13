using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Api.Migrations
{
    public partial class updateconfigurationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "Configurations",
                newName: "RequestedBy");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Configurations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeployedBy",
                table: "Configurations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "DeployedBy",
                table: "Configurations");

            migrationBuilder.RenameColumn(
                name: "RequestedBy",
                table: "Configurations",
                newName: "User");
        }
    }
}
