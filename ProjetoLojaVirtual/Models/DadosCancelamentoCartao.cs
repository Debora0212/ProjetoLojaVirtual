using ProjetoLojaVirtual.Libraries.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class DadosCancelamentoCartao
    {
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string Motivo { get; set; }

        public string FormPagamento { get; set; }
    }
}
