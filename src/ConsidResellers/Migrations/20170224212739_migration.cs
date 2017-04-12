using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsidResellers.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    OrganizationNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Address = table.Column<string>(maxLength: 512, nullable: false),
                    City = table.Column<string>(maxLength: 512, nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    Country = table.Column<string>(maxLength: 50, nullable: false),
                    Latitude = table.Column<string>(maxLength: 15, nullable: true),
                    Longitude = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Zip = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Stores",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CompanyId",
                table: "Stores",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
