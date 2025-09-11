using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProSolution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class seart11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureOptionItems_FeatureOptionItems_ParentId",
                table: "FeatureOptionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_FeatureOptionItems_FeatureOptionItemId",
                table: "ProductFeatures");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureOptionItems_FeatureOptionItems_ParentId",
                table: "FeatureOptionItems",
                column: "ParentId",
                principalTable: "FeatureOptionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_FeatureOptionItems_FeatureOptionItemId",
                table: "ProductFeatures",
                column: "FeatureOptionItemId",
                principalTable: "FeatureOptionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureOptionItems_FeatureOptionItems_ParentId",
                table: "FeatureOptionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_FeatureOptionItems_FeatureOptionItemId",
                table: "ProductFeatures");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureOptionItems_FeatureOptionItems_ParentId",
                table: "FeatureOptionItems",
                column: "ParentId",
                principalTable: "FeatureOptionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_FeatureOptionItems_FeatureOptionItemId",
                table: "ProductFeatures",
                column: "FeatureOptionItemId",
                principalTable: "FeatureOptionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
