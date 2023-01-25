using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanchesMac.Migrations
{
    public partial class PopulandoCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorias(categoriaNome, Descricao) " +
                "VALUES('Normal', 'Lanches feito com ingredientes normais')");
            migrationBuilder.Sql("INSERT INTO Categorias(categoriaNome, Descricao) " +
                "VALUES('Natural', 'Lanches feito com ingredientes naturais')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
        }
    }
}
