using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tpcUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AUserType_Id",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_ACompanyOwner_CompanyOwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyOwners_ACompanyOwner_Id",
                table: "CompanyOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeMembers_AHomeUser_UserId",
                table: "HomeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Homes_AHomeUser_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeUsers_AHomeUser_Id",
                table: "HomeUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AHomeUser_AHomeUserId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "ACompanyOwner");

            migrationBuilder.DropTable(
                name: "AHomeUser");

            migrationBuilder.DropTable(
                name: "AUserType");

            migrationBuilder.CreateSequence(
                name: "AUserTypeSequence");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HomeUsers",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [AUserTypeSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AHomeId",
                table: "HomeUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "HomeUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "HomeUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CompanyOwners",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [AUserTypeSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CompanyOwners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CompanyOwners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR [AUserTypeSequence]",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Admins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HomeUsers_AHomeId",
                table: "HomeUsers",
                column: "AHomeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeUsers_UserId",
                table: "HomeUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyOwners_CompanyId",
                table: "CompanyOwners",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyOwners_UserId",
                table: "CompanyOwners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HomeUsers_AHomeId",
                table: "HomeUsers");

            migrationBuilder.DropIndex(
                name: "IX_HomeUsers_UserId",
                table: "HomeUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyOwners_CompanyId",
                table: "CompanyOwners");

            migrationBuilder.DropIndex(
                name: "IX_CompanyOwners_UserId",
                table: "CompanyOwners");

            migrationBuilder.DropIndex(
                name: "IX_Admins_UserId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "AHomeId",
                table: "HomeUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "HomeUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HomeUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CompanyOwners");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CompanyOwners");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Admins");

            migrationBuilder.DropSequence(
                name: "AUserTypeSequence");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HomeUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [AUserTypeSequence]");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CompanyOwners",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [AUserTypeSequence]");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Admins",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR [AUserTypeSequence]");

            migrationBuilder.CreateTable(
                name: "ACompanyOwner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACompanyOwner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AHomeUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AHomeId = table.Column<int>(type: "int", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AHomeUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AUserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUserType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACompanyOwner_CompanyId",
                table: "ACompanyOwner",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AHomeUser_AHomeId",
                table: "AHomeUser",
                column: "AHomeId");

            migrationBuilder.CreateIndex(
                name: "IX_AUserType_UserId",
                table: "AUserType",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AUserType_Id",
                table: "Admins",
                column: "Id",
                principalTable: "AUserType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_ACompanyOwner_CompanyOwnerId",
                table: "Companies",
                column: "CompanyOwnerId",
                principalTable: "ACompanyOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyOwners_ACompanyOwner_Id",
                table: "CompanyOwners",
                column: "Id",
                principalTable: "ACompanyOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeMembers_AHomeUser_UserId",
                table: "HomeMembers",
                column: "UserId",
                principalTable: "AHomeUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_AHomeUser_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId",
                principalTable: "AHomeUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeUsers_AHomeUser_Id",
                table: "HomeUsers",
                column: "Id",
                principalTable: "AHomeUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AHomeUser_AHomeUserId",
                table: "Notifications",
                column: "AHomeUserId",
                principalTable: "AHomeUser",
                principalColumn: "Id");
        }
    }
}
