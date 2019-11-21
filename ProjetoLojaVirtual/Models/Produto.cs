using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class Produto
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        
        //Frete-Correios
        public double Peso { get; set; }
        public int Largura { get; set; }
        public int Altura { get; set; }
        public int Comprimento { get; set; }

        /*
         * Entity Framework -ORM - Biblioteca unir - banco de dados e POO (ORM - Mapeamentos de objetos relacionados)
         *Por meio de Fluent API ou Attributes
         */

        //Banco de dados - Relacionamento entre tabela
        public int CategoriaId { get; set; }

        //Programação orientada ao objeto -Associação entre objetos
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }

       
        public virtual ICollection<Imagem> Imagens { get; set; }
     

    }

}
