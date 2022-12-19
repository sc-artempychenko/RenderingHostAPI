using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class Updatedrenderinghostprimarykey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RenderingHosts",
                table: "RenderingHosts");

            migrationBuilder.DropColumn(
                name: "SourceControlIntegrationName",
                table: "RenderingHosts");

            migrationBuilder.AlterColumn<string>(
                name: "RenderingHostId",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RenderingHosts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RenderingHosts",
                table: "RenderingHosts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RenderingHosts",
                table: "RenderingHosts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RenderingHosts");

            migrationBuilder.AlterColumn<string>(
                name: "RenderingHostId",
                table: "RenderingHosts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceControlIntegrationName",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RenderingHosts",
                table: "RenderingHosts",
                column: "RenderingHostId");
        }
    }
}
