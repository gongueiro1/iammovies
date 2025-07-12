using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iammovies.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "AspNetUsers",
                newName: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "AspNetUsers",
                newName: "nome");
        }
    }
}
