using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfinance.Core.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FiMaAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<int>(type: "int", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    IsBillWise = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBranchId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Parent = table.Column<int>(type: "int", nullable: true),
                    IsGroup = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    FinalAccount = table.Column<int>(type: "int", nullable: false),
                    SortField = table.Column<int>(type: "int", nullable: true),
                    AccountGroup = table.Column<int>(type: "int", nullable: false),
                    SubGroup = table.Column<int>(type: "int", nullable: true),
                    SystemAccount = table.Column<bool>(type: "bit", nullable: false),
                    AccountCategory = table.Column<int>(type: "int", nullable: true),
                    GroupOrder = table.Column<int>(type: "int", nullable: true),
                    PreventExtraPay = table.Column<bool>(type: "bit", nullable: true),
                    IsItemwise = table.Column<bool>(type: "bit", nullable: true),
                    IsCollection = table.Column<bool>(type: "bit", nullable: true),
                    IsCostCentre = table.Column<bool>(type: "bit", nullable: true),
                    ValueOfGoods = table.Column<bool>(type: "bit", nullable: true),
                    AlternateName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiMaAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeType = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocationRestrictedUser = table.Column<bool>(type: "bit", nullable: false),
                    PhotoId = table.Column<int>(type: "int", nullable: true),
                    CreateBranchId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccoutnId = table.Column<int>(type: "int", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaEmployees_FiMaAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "FiMaAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaEmployees_AccountId",
                table: "MaEmployees",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaEmployees");

            migrationBuilder.DropTable(
                name: "FiMaAccounts");
        }
    }
}
