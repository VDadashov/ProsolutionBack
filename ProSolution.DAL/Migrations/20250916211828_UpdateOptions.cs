using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProSolution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "FeatureOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "FeatureOptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeatureOptions_ParentId",
                table: "FeatureOptions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureOptions_FeatureOptions_ParentId",
                table: "FeatureOptions",
                column: "ParentId",
                principalTable: "FeatureOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureOptions_FeatureOptions_ParentId",
                table: "FeatureOptions");

            migrationBuilder.DropIndex(
                name: "IX_FeatureOptions_ParentId",
                table: "FeatureOptions");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "FeatureOptions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "FeatureOptions");
        }
    }
}
