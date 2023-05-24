using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth.Migrations
{
    public partial class spellcheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaseMeterial",
                table: "Products",
                newName: "CaseMaterial");

            migrationBuilder.RenameColumn(
                name: "Quanlity",
                table: "ImportDetails",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaseMaterial",
                table: "Products",
                newName: "CaseMeterial");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ImportDetails",
                newName: "Quanlity");
        }
    }
}
