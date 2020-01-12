using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoLojaVirtual.Migrations
{
    public partial class ProdutoRenomearQuantidadeEstoque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Quantidade", "Produtos", "Estoque");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Estoque", "Produtos", "Quantidade");
        }
    }
}
