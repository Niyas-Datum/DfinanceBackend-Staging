using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfinance.AuthCore.AuthCoreMigrations
{
    /// <inheritdoc />
    public partial class dfMasterinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DatabaseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServerIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRemote = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BussinessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CompanySchedule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySchedule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CompanySchedule_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserCompany",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompany", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserCompany_Company",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserCompany_Users",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company",
                table: "Company",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_1",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanySchedule_CompanyID",
                table: "CompanySchedule",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Users",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCompany",
                table: "UserCompany",
                columns: new[] { "CompanyID", "UserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCompany_UserID",
                table: "UserCompany",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySchedule");

            migrationBuilder.DropTable(
                name: "UserCompany");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
