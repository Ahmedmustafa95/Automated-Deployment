using Microsoft.EntityFrameworkCore.Migrations;

namespace AutomatedDeployment.Infrastructure.Migrations
{
    public partial class AddConfigPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfigFilepPath",
                table: "HubsApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigFilepPath",
                table: "HubsApplications");
        }
    }
}
