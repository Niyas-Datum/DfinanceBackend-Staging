using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfinance.Core.Migrations
{
    /// <inheritdoc />
    public partial class Macompany_Department_ReDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaEmployees_FiMaAccounts_AccountId",
                table: "MaEmployees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaEmployees",
                table: "MaEmployees");

            migrationBuilder.DropColumn(
                name: "AccoutnId",
                table: "MaEmployees");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "MaEmployees",
                newName: "AccountID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MaEmployees",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CreateBranchId",
                table: "MaEmployees",
                newName: "CreatedBranchID");

            migrationBuilder.RenameIndex(
                name: "IX_MaEmployees_AccountId",
                table: "MaEmployees",
                newName: "IX_MaEmployees_AccountID");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "MaEmployees",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "MaEmployees",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "MaCompanies",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactPersonID = table.Column<int>(type: "int", nullable: true),
                    Nature = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    AddressLineOne = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AddressLineTwo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    POBox = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TelephoneNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FaxNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActiveFlag = table.Column<byte>(type: "tinyint", nullable: false),
                    BranchCompanyID = table.Column<int>(type: "int", nullable: true),
                    SalesTaxNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CentralSalesTaxNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UniqueID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DL1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DL2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LockSystem = table.Column<bool>(type: "bit", nullable: true),
                    ArabicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HOCompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HOCompanyNameArabic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountBranchID = table.Column<int>(type: "int", nullable: true),
                    BulidingNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaCompanies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MaCompanies_MaCompanies1",
                        column: x => x.BranchCompanyID,
                        principalTable: "MaCompanies",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MaCompanies_MaEmployees",
                        column: x => x.ContactPersonID,
                        principalTable: "MaEmployees",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MaCompanies_MaEmployees1",
                        column: x => x.CreatedBy,
                        principalTable: "MaEmployees",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ReDepartmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActiveFlag = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReDepartmentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReDepartmentTypes_MaCompanies_CreatedBranchId",
                        column: x => x.CreatedBranchId,
                        principalTable: "MaCompanies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaDepartments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentTypeID = table.Column<int>(type: "int", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActiveFlag = table.Column<byte>(type: "tinyint", nullable: false),
                    MaEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaDepartments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MaDepartments_MaCompanies",
                        column: x => x.CompanyID,
                        principalTable: "MaCompanies",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MaDepartments_MaDepartments",
                        column: x => x.DepartmentTypeID,
                        principalTable: "ReDepartmentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaDepartments_MaEmployees_MaEmployeeId",
                        column: x => x.MaEmployeeId,
                        principalTable: "MaEmployees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaEmployees",
                table: "MaEmployees",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaCompanies_BranchCompanyID",
                table: "MaCompanies",
                column: "BranchCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_MaCompanies_ContactPersonID",
                table: "MaCompanies",
                column: "ContactPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_MaCompanies_CreatedBy",
                table: "MaCompanies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaDepartments_CompanyID",
                table: "MaDepartments",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_MaDepartments_DepartmentTypeID",
                table: "MaDepartments",
                column: "DepartmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MaDepartments_MaEmployeeId",
                table: "MaDepartments",
                column: "MaEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReDepartmentTypes_CreatedBranchId",
                table: "ReDepartmentTypes",
                column: "CreatedBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaEmployees_FiMaAccounts_AccountID",
                table: "MaEmployees",
                column: "AccountID",
                principalTable: "FiMaAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaEmployees_FiMaAccounts_AccountID",
                table: "MaEmployees");

            migrationBuilder.DropTable(
                name: "MaDepartments");

            migrationBuilder.DropTable(
                name: "ReDepartmentTypes");

            migrationBuilder.DropTable(
                name: "MaCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "MaEmployees");

            migrationBuilder.DropIndex(
                name: "IX_MaEmployees",
                table: "MaEmployees");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "MaEmployees",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "MaEmployees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CreatedBranchID",
                table: "MaEmployees",
                newName: "CreateBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_MaEmployees_AccountID",
                table: "MaEmployees",
                newName: "IX_MaEmployees_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "MaEmployees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "AccoutnId",
                table: "MaEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaEmployees",
                table: "MaEmployees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaEmployees_FiMaAccounts_AccountId",
                table: "MaEmployees",
                column: "AccountId",
                principalTable: "FiMaAccounts",
                principalColumn: "Id");
        }
    }
}
