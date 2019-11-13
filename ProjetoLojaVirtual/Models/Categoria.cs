using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        /*
         * Nome: Telefone sem fio
         * Slug: telefone-sem-fio
         */
        public string Slug { get; set; }


        /*
         * Auto-relacionamento
         * 1-Informatica - P=null
         * - 2-Mouse - P=1
         * -- 3-Mouse sem fio - P=2
         * -- 4-Mouse gamer - P=2
         */
        public int? CategoriaPaiId { get; set; }

         /*
         * ORM - EntityFrameworkCore
         */
         [ForeignKey("CategoriaPaiId")]
        public virtual Categoria CategoriaPai { get; set; }
    }
}
