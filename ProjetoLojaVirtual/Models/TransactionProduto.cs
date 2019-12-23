using PagarMe;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class TransactionProduto
    {
        public TransacaoPagarMe Transaction { get; set; }
        public List<ProdutoItem> Produtos { get; set; }
    }
}
