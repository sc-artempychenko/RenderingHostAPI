using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class removedplatformtenantname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlatformTenantName",
                table: "RenderingHosts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlatformTenantName",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
