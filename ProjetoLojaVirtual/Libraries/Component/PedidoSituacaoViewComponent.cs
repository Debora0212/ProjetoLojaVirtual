using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Component
{
    public class PedidoSituacaoViewComponent : ViewComponent
    {
        List<PedidoSituacaoStatus> Timeline1 { get; set; }
        List<string> StatusTimeLine1 = new List<string>()
        {
                PedidoSituacaoConstant.PEDIDO_REALIZADO,
                PedidoSituacaoConstant.PAGAMENTO_APROVADO,
                PedidoSituacaoConstant.NF_EMITIDA,
                PedidoSituacaoConstant.EM_TRANSPORTE,
                PedidoSituacaoConstant.ENTREGUE,
                PedidoSituacaoConstant.FINALIZADO,
        };

        List<PedidoSituacaoStatus> Timeline2 { get; set; }
        List<string> StatusTimeLine2 = new List<string>()
        {
                PedidoSituacaoConstant.PAGAMENTO_NAO_REALIZADO,
        };

        List<PedidoSituacaoStatus> Timeline3 { get; set; }
        List<string> StatusTimeLine3 = new List<string>()
        {
                PedidoSituacaoConstant.ESTORNO,
        };

        List<PedidoSituacaoStatus> Timeline4 { get; set; }
        List<string> StatusTimeLine4 = new List<string>()
        {
                PedidoSituacaoConstant.DEVOLVER,
                PedidoSituacaoConstant.DEVOLVER_ENTREGUE,
                PedidoSituacaoConstant.DEVOLUCAO_ACEITA,
                PedidoSituacaoConstant.DEVOLVER_ESTORNO,
        };

        List<PedidoSituacaoStatus> Timeline5 { get; set; }
        List<string> StatusTimeLine5 = new List<string>()
        {
                PedidoSituacaoConstant.DEVOLUCAO_REJEITADA,
             
        };


        public PedidoSituacaoViewComponent()
        {
            Timeline1 = new List<PedidoSituacaoStatus>();
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.EM_TRANSPORTE, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ENTREGUE, Concluido = false, Cor = "complete" });
            Timeline1.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.FINALIZADO, Concluido = false, Cor = "complete" });

            Timeline2 = new List<PedidoSituacaoStatus>();
            Timeline2.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline2.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_NAO_REALIZADO, Concluido = false, Cor = "complete-red" });

            Timeline3 = new List<PedidoSituacaoStatus>();
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false, Cor = "complete" });
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false, Cor = "complete" });
            Timeline3.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ESTORNO, Concluido = false, Cor = "complete-red" });

            Timeline4 = new List<PedidoSituacaoStatus>();
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.EM_TRANSPORTE, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ENTREGUE, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLVER, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLVER_ENTREGUE, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLUCAO_ACEITA, Concluido = false, Cor = "complete" });
            Timeline4.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLVER_ESTORNO, Concluido = false, Cor = "complete" });

            Timeline5 = new List<PedidoSituacaoStatus>();
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PEDIDO_REALIZADO, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.NF_EMITIDA, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.EM_TRANSPORTE, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.ENTREGUE, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLVER, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLVER_ENTREGUE, Concluido = false, Cor = "complete" });
            Timeline5.Add(new PedidoSituacaoStatus() { Situacao = PedidoSituacaoConstant.DEVOLUCAO_REJEITADA, Concluido = false, Cor = "complete" });

        }

        public async Task<IViewComponentResult> InvokeAsync(Pedido pedido)
        {
            List<PedidoSituacaoStatus> timeline = null;

            if (StatusTimeLine1.Contains(pedido.Situacao))
            {
                timeline = Timeline1;
            }
            if (StatusTimeLine2.Contains(pedido.Situacao))
            {
                timeline = Timeline2;
            }
            if (StatusTimeLine3.Contains(pedido.Situacao))
            {
                timeline = Timeline3;

                var nfe = pedido.PedidoSituacoes.Where(a => a.Situacao == PedidoSituacaoConstant.NF_EMITIDA).FirstOrDefault();
                if (nfe == null)
                {
                    timeline.Remove(timeline.FirstOrDefault(a => a.Situacao == PedidoSituacaoConstant.NF_EMITIDA));
                }
            }
            if (StatusTimeLine4.Contains(pedido.Situacao))
            {
                timeline = Timeline4;
            }
            if (StatusTimeLine5.Contains(pedido.Situacao))
            {
                timeline = Timeline5;
            }
            if (timeline != null)
            {
                foreach (var pedidoSituacao in pedido.PedidoSituacoes)
                {
                    var pedidoSituacaoTimeline = timeline.Where(a => a.Situacao == pedidoSituacao.Situacao).FirstOrDefault();
                    pedidoSituacaoTimeline.Data = pedidoSituacao.Data;
                    pedidoSituacaoTimeline.Concluido = true;
                }

            }

            return View(timeline);
        }
    }
}
