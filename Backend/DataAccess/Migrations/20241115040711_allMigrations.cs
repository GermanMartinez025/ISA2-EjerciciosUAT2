using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class allMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ACompanySequence");

            migrationBuilder.CreateSequence(
                name: "AHomeSequence");

            migrationBuilder.CreateSequence(
                name: "ANotificationSequence");

            migrationBuilder.CreateSequence(
                name: "ARoomSequence");

            migrationBuilder.CreateSequence(
                name: "AUserSequence");

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
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AHomeId = table.Column<int>(type: "int", nullable: true)
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
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ARoomSequence]"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [AUserSequence]"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ACompanySequence]"),
                    Rut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyOwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_ACompanyOwner_CompanyOwnerId",
                        column: x => x.CompanyOwnerId,
                        principalTable: "ACompanyOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyOwners_ACompanyOwner_Id",
                        column: x => x.Id,
                        principalTable: "ACompanyOwner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeMembers",
                columns: table => new
                {
                    HomeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Notifiable = table.Column<bool>(type: "bit", nullable: false),
                    ListDevices = table.Column<bool>(type: "bit", nullable: false),
                    AddDevices = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDevices = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeMembers", x => new { x.HomeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_HomeMembers_AHomeUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AHomeUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [AHomeSequence]"),
                    MainStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoorNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: false),
                    HomeOwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homes_AHomeUser_HomeOwnerId",
                        column: x => x.HomeOwnerId,
                        principalTable: "AHomeUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HomeUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeUsers_AHomeUser_Id",
                        column: x => x.Id,
                        principalTable: "AHomeUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ANotificationSequence]"),
                    Event = table.Column<int>(type: "int", nullable: false),
                    DeviceHardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserHomeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AHomeMemberHomeId = table.Column<int>(type: "int", nullable: true),
                    AHomeMemberUserId = table.Column<int>(type: "int", nullable: true),
                    AHomeUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AHomeUser_AHomeUserId",
                        column: x => x.AHomeUserId,
                        principalTable: "AHomeUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_AUserType_Id",
                        column: x => x.Id,
                        principalTable: "AUserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OutsideEnvironment = table.Column<bool>(type: "bit", nullable: false),
                    MovementDetection = table.Column<bool>(type: "bit", nullable: false),
                    PersonDetection = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeDevices",
                columns: table => new
                {
                    HardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    HomeId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Online = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeDevices", x => x.HardwareId);
                    table.ForeignKey(
                        name: "FK_HomeDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MotionSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotionSensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotionSensors_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartLamps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartLamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartLamps_Devices_Id",
                        column: x => x.Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_AUserType_UserId1",
                table: "AUserType",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyOwnerId",
                table: "Companies",
                column: "CompanyOwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_DeviceId",
                table: "HomeDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_HomeId",
                table: "HomeDevices",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_RoomId",
                table: "HomeDevices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeMembers_UserId",
                table: "HomeMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AHomeMemberHomeId_AHomeMemberUserId",
                table: "Notifications",
                columns: new[] { "AHomeMemberHomeId", "AHomeMemberUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AHomeUserId",
                table: "Notifications",
                column: "AHomeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeviceHardwareId",
                table: "Notifications",
                column: "DeviceHardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserHomeId_UserId",
                table: "Notifications",
                columns: new[] { "UserHomeId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HomeId",
                table: "Rooms",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyOwners");

            migrationBuilder.DropTable(
                name: "HomeDevices");

            migrationBuilder.DropTable(
                name: "HomeMembers");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "HomeUsers");

            migrationBuilder.DropTable(
                name: "MotionSensors");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "SmartLamps");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AUserType");

            migrationBuilder.DropTable(
                name: "ACompanyOwner");

            migrationBuilder.DropTable(
                name: "AHomeUser");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropSequence(
                name: "ACompanySequence");

            migrationBuilder.DropSequence(
                name: "AHomeSequence");

            migrationBuilder.DropSequence(
                name: "ANotificationSequence");

            migrationBuilder.DropSequence(
                name: "ARoomSequence");

            migrationBuilder.DropSequence(
                name: "AUserSequence");
        }
    }
}
