using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedrenderinghosttablewithnewtypeofhostingmethodfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RenderingHostHostingMethod",
                table: "RenderingHosts");

            migrationBuilder.AddColumn<string>(
                name: "RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HostingMethodDTO",
                columns: table => new
                {
                    RenderingHostName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HostingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostingMethodDTO", x => x.RenderingHostName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RenderingHosts_RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts",
                column: "RenderingHostHostingMethodRenderingHostName");

            migrationBuilder.AddForeignKey(
                name: "FK_RenderingHosts_HostingMethodDTO_RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts",
                column: "RenderingHostHostingMethodRenderingHostName",
                principalTable: "HostingMethodDTO",
                principalColumn: "RenderingHostName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenderingHosts_HostingMethodDTO_RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts");

            migrationBuilder.DropTable(
                name: "HostingMethodDTO");

            migrationBuilder.DropIndex(
                name: "IX_RenderingHosts_RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts");

            migrationBuilder.DropColumn(
                name: "RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts");

            migrationBuilder.AddColumn<string>(
                name: "RenderingHostHostingMethod",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
