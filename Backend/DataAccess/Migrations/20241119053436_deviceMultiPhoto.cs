using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deviceMultiPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photos",
                table: "Devices",
                newName: "MainPhoto");

            migrationBuilder.CreateTable(
                name: "DevicePhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    ADeviceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevicePhoto_Devices_ADeviceId",
                        column: x => x.ADeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DevicePhoto_ADeviceId",
                table: "DevicePhoto",
                column: "ADeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevicePhoto");

            migrationBuilder.RenameColumn(
                name: "MainPhoto",
                table: "Devices",
                newName: "Photos");
        }
    }
}
