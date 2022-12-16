using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedrenderinghosttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RenderingHosts",
                columns: table => new
                {
                    RenderingHostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceControlIntegrationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepositoryUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformTenantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnvironmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenderingHostHostingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenderingHosts", x => x.RenderingHostId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenderingHosts");
        }
    }
}
