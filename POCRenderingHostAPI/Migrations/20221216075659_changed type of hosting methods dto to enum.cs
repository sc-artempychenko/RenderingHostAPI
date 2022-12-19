using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class changedtypeofhostingmethodsdtotoenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HostingMethodDTO");

            migrationBuilder.AddColumn<int>(
                name: "RenderingHostHostingMethod",
                table: "RenderingHosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RenderingHostHostingMethod",
                table: "RenderingHosts");

            migrationBuilder.CreateTable(
                name: "HostingMethodDTO",
                columns: table => new
                {
                    RenderingHostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HostingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostingMethodDTO", x => x.RenderingHostId);
                    table.ForeignKey(
                        name: "FK_HostingMethodDTO_RenderingHosts_RenderingHostId",
                        column: x => x.RenderingHostId,
                        principalTable: "RenderingHosts",
                        principalColumn: "RenderingHostId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
