using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedrenderinghosttablewithhostfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RenderingHostUrl",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RenderingHostUrl",
                table: "RenderingHosts");
        }
    }
}
