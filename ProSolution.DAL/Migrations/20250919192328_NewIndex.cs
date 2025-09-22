using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProSolution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "ProductFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "ProductFeatures");
        }
    }
}
