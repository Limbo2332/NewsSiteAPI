using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsSite.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsRubrics_News_PieceOfNewsId",
                table: "NewsRubrics");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsTags_News_PieceOfNewsId",
                table: "NewsTags");

            migrationBuilder.RenameColumn(
                name: "PieceOfNewsId",
                table: "NewsTags",
                newName: "NewsId");

            migrationBuilder.RenameColumn(
                name: "PieceOfNewsId",
                table: "NewsRubrics",
                newName: "NewsId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_Name",
                table: "Rubrics",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsRubrics_News_NewsId",
                table: "NewsRubrics",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTags_News_NewsId",
                table: "NewsTags",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsRubrics_News_NewsId",
                table: "NewsRubrics");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsTags_News_NewsId",
                table: "NewsTags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Rubrics_Name",
                table: "Rubrics");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "NewsTags",
                newName: "PieceOfNewsId");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "NewsRubrics",
                newName: "PieceOfNewsId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Authors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsRubrics_News_PieceOfNewsId",
                table: "NewsRubrics",
                column: "PieceOfNewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTags_News_PieceOfNewsId",
                table: "NewsTags",
                column: "PieceOfNewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
