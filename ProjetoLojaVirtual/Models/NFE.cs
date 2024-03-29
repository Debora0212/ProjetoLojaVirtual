﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ProjetoLojaVirtual.Libraries.Lang;

namespace ProjetoLojaVirtual.Models
{
    public class NFE
    {
        [Url(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E004")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        public string NFE_URL { get; set; }
    }
}
