using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoLojaVirtual.Migrations
{
    public partial class PedidoPedidoSituacao2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dadosprodutos",
                table: "Pedidos",
                newName: "DadosProdutos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DadosProdutos",
                table: "Pedidos",
                newName: "Dadosprodutos");
        }
    }
}
