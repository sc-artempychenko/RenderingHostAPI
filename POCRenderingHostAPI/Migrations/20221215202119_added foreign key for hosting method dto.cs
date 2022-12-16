using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedforeignkeyforhostingmethoddto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenderingHosts_HostingMethodDTO_RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts");

            migrationBuilder.DropIndex(
                name: "IX_RenderingHosts_RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts");

            migrationBuilder.DropColumn(
                name: "RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RenderingHosts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RenderingHosts_Name",
                table: "RenderingHosts",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_RenderingHosts_HostingMethodDTO_Name",
                table: "RenderingHosts",
                column: "Name",
                principalTable: "HostingMethodDTO",
                principalColumn: "RenderingHostName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenderingHosts_HostingMethodDTO_Name",
                table: "RenderingHosts");

            migrationBuilder.DropIndex(
                name: "IX_RenderingHosts_Name",
                table: "RenderingHosts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RenderingHostHostingMethodRenderingHostName",
                table: "RenderingHosts",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
