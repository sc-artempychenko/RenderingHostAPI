using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedforeignkeyforhostingmethoddtoasonetoonerelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenderingHosts_HostingMethodDTO_Name",
                table: "RenderingHosts");

            migrationBuilder.DropIndex(
                name: "IX_RenderingHosts_Name",
                table: "RenderingHosts");

            migrationBuilder.RenameColumn(
                name: "RenderingHostName",
                table: "HostingMethodDTO",
                newName: "RenderingHostId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RenderingHosts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HostingMethodDTO_RenderingHosts_RenderingHostId",
                table: "HostingMethodDTO",
                column: "RenderingHostId",
                principalTable: "RenderingHosts",
                principalColumn: "RenderingHostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HostingMethodDTO_RenderingHosts_RenderingHostId",
                table: "HostingMethodDTO");

            migrationBuilder.RenameColumn(
                name: "RenderingHostId",
                table: "HostingMethodDTO",
                newName: "RenderingHostName");

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
    }
}
