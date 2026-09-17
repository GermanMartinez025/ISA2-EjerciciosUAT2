using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class useDeviceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevicePhoto_Devices_ADeviceId",
                table: "DevicePhoto");

            migrationBuilder.DropIndex(
                name: "IX_DevicePhoto_ADeviceId",
                table: "DevicePhoto");

            migrationBuilder.DropColumn(
                name: "ADeviceId",
                table: "DevicePhoto");

            migrationBuilder.CreateIndex(
                name: "IX_DevicePhoto_DeviceId",
                table: "DevicePhoto",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevicePhoto_Devices_DeviceId",
                table: "DevicePhoto",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevicePhoto_Devices_DeviceId",
                table: "DevicePhoto");

            migrationBuilder.DropIndex(
                name: "IX_DevicePhoto_DeviceId",
                table: "DevicePhoto");

            migrationBuilder.AddColumn<int>(
                name: "ADeviceId",
                table: "DevicePhoto",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DevicePhoto_ADeviceId",
                table: "DevicePhoto",
                column: "ADeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevicePhoto_Devices_ADeviceId",
                table: "DevicePhoto",
                column: "ADeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }
    }
}
