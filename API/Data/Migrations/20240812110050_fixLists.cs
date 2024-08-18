using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyLists_AspNetUsers_AuthorId",
                table: "FamilyLists");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyLists_Categories_CategoryId",
                table: "FamilyLists");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyLists_Families_FamilyId",
                table: "FamilyLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FamilyLists",
                table: "FamilyLists");

            migrationBuilder.DropColumn(
                name: "AuthorUsername",
                table: "FamilyLists");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "FamilyLists");

            migrationBuilder.RenameTable(
                name: "FamilyLists",
                newName: "Lists");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyLists_FamilyId",
                table: "Lists",
                newName: "IX_Lists_FamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyLists_CategoryId",
                table: "Lists",
                newName: "IX_Lists_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyLists_AuthorId",
                table: "Lists",
                newName: "IX_Lists_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lists",
                table: "Lists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_AspNetUsers_AuthorId",
                table: "Lists",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Categories_CategoryId",
                table: "Lists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Families_FamilyId",
                table: "Lists",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lists_AspNetUsers_AuthorId",
                table: "Lists");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Categories_CategoryId",
                table: "Lists");

            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Families_FamilyId",
                table: "Lists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lists",
                table: "Lists");

            migrationBuilder.RenameTable(
                name: "Lists",
                newName: "FamilyLists");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_FamilyId",
                table: "FamilyLists",
                newName: "IX_FamilyLists_FamilyId");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_CategoryId",
                table: "FamilyLists",
                newName: "IX_FamilyLists_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Lists_AuthorId",
                table: "FamilyLists",
                newName: "IX_FamilyLists_AuthorId");

            migrationBuilder.AddColumn<string>(
                name: "AuthorUsername",
                table: "FamilyLists",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "FamilyLists",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FamilyLists",
                table: "FamilyLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyLists_AspNetUsers_AuthorId",
                table: "FamilyLists",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyLists_Categories_CategoryId",
                table: "FamilyLists",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyLists_Families_FamilyId",
                table: "FamilyLists",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
