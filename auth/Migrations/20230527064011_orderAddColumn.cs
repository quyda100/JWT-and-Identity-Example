using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth.Migrations
{
    public partial class orderAddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "Orders");
        }
    }
}
