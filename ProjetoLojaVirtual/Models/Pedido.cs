using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class Pedido
    {
        //Chave Primaria - PK
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public string TransactionId { get; set; } //Pagar.Me - Transaction -> Gera um Id.

        //Frete
        public string FreteEmpresa { get; set; } //ECT - Correios
        public string FreteCodRastreamento { get; set; }

        public string FormPagamento { get; set; } //Boleto - Bancario e Cartao de credito
        public decimal ValorTotal { get; set; }
        public string DadosTransaction { get; set; } //Transaction em formato JSON
        public string DadosProdutos { get; set; } // ProdutoItem em formato JSON (Alterei de Dadosprodutos para DadosProdutos

        public DateTime DataRegistro { get; set; }
        public string Situacao { get; set; }

        //URL - Com site da Receita - Nota Fiscal
        public string NFE { get; set; }
        
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("PedidoId")]
        public virtual ICollection<PedidoSituacao> PedidoSituacoes { get; set; }

    }
}
