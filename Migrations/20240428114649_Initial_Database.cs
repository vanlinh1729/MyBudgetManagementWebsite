using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Database : Migration
    {
        /// <inheritdoc />
        // Inside the migration's Up method
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "Roles",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Name = table.Column<int>(type: "int", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Roles", x => x.Id);
        });

    migrationBuilder.CreateTable(
        name: "Users",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Users", x => x.Id);
        });

    migrationBuilder.CreateTable(
        name: "Category",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            UserBalanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Category", x => x.Id);
            table.ForeignKey(
                name: "FK_Category_Users_UserId",
                column: x => x.UserId,
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); // Leave this as CascadeType.Cascade
        });

    migrationBuilder.CreateTable(
        name: "UserBalances",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_UserBalances", x => x.Id);
            table.ForeignKey(
                name: "FK_UserBalances_Users_UserId",
                column: x => x.UserId,
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // Change this to ReferentialAction.Restrict
        });

    migrationBuilder.CreateTable(
        name: "UserRoles",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_UserRoles", x => x.Id);
            table.ForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                column: x => x.RoleId,
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                name: "FK_UserRoles_Users_UserId",
                column: x => x.UserId,
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateTable(
        name: "Transactions",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Type = table.Column<int>(type: "int", nullable: false),
            Date = table.Column<DateTime>(type: "datetime2", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Transactions", x => x.Id);
            table.ForeignKey(
                name: "FK_Transactions_Category_CategoryId",
                column: x => x.CategoryId,
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                name: "FK_Transactions_Users_UserId",
                column: x => x.UserId,
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateIndex(
        name: "IX_Category_UserId",
        table: "Category",
        column: "UserId");

    migrationBuilder.CreateIndex(
        name: "IX_Transactions_CategoryId",
        table: "Transactions",
        column: "CategoryId");

    migrationBuilder.CreateIndex(
        name: "IX_Transactions_UserId",
        table: "Transactions",
        column: "UserId");

    migrationBuilder.CreateIndex(
        name: "IX_UserBalances_UserId",
        table: "UserBalances",
        column: "UserId",
        unique: true);

    migrationBuilder.CreateIndex(
        name: "IX_UserRoles_RoleId",
        table: "UserRoles",
        column: "RoleId");

    migrationBuilder.CreateIndex(
        name: "IX_UserRoles_UserId",
        table: "UserRoles",
        column: "UserId");
}

// Your Down method remains unchanged
protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropTable(
        name: "Transactions");

    migrationBuilder.DropTable(
        name: "UserBalances");

    migrationBuilder.DropTable(
        name: "UserRoles");

    migrationBuilder.DropTable(
        name: "Category");

    migrationBuilder.DropTable(
        name: "Roles");

    migrationBuilder.DropTable(
        name: "Users");
    
}
    }
}
