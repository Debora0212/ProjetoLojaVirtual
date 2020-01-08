using ProjetoLojaVirtual.Libraries.Lang;
using ProjetoLojaVirtual.Libraries.Validacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class Categoria
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        //TODO-Criar validacao -Nome categoria unico no banco de dados
        
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        [NomeCategoriaUnico(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E011")]
        public string Nome { get; set; }

        /*
         * Nome: Telefone sem fio
         * Slug: telefone-sem-fio
         */
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        public string Slug { get; set; }


        /*
         * Auto-relacionamento
         * 1-Informatica - P=null
         * - 2-Mouse - P=1
         * -- 3-Mouse sem fio - P=2
         * -- 4-Mouse gamer - P=2
         */
        [Display(Name = "Categoria Pai")]
        public int? CategoriaPaiId { get; set; }

         /*
         * ORM - EntityFrameworkCore
         */
         [ForeignKey("CategoriaPaiId")]
        public virtual Categoria CategoriaPai { get; set; }
    }
}
