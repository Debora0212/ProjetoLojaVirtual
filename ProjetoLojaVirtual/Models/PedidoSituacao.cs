﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Models
{
    public class PedidoSituacao
    {
        //PK
        public int Id { get; set; }

        [ForeignKey("Pedido")]
        public int? PedidoId { get; set; }

        public DateTime Data { get; set; }
        public string Situacao { get; set; }
        public string Dados { get; set; } //JSON - Pagar.ME


        public virtual Pedido Pedido { get; set; }
    }
}
