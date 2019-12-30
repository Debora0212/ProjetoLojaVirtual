﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;

        public PedidoController(IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
        }

        public IActionResult Index(int? pagina, string codigoPedido, string cpf)
        {
            var pedidos = _pedidoRepository.ObterTodosPedido(pagina, codigoPedido, cpf);

            return View(pedidos);
        }

        public IActionResult Visualizar(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            return View(pedido);
        }

        public IActionResult NFE(int id)
        {
            string url = HttpContext.Request.Form["nfe_url"];

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            pedido.NFE = url;
            pedido.Situacao = PedidoSituacaoConstant.NF_EMITIDA;

            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            pedidoSituacao.Dados = url;
            pedidoSituacao.PedidoId = id;
            pedidoSituacao.Situacao = PedidoSituacaoConstant.NF_EMITIDA;

            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

            _pedidoRepository.Atualizar(pedido);

            return RedirectToAction(nameof(Visualizar), new { id = id });
        }
        public IActionResult RegistrarRastreamento(int id)
        {
            string codRastreamento = HttpContext.Request.Form["cod_rastreamento"];

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            pedido.FreteCodRastreamento = codRastreamento;
            pedido.Situacao = PedidoSituacaoConstant.EM_TRANSPORTE;

            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            pedidoSituacao.Dados = codRastreamento;
            pedidoSituacao.PedidoId = id;
            pedidoSituacao.Situacao = PedidoSituacaoConstant.EM_TRANSPORTE;

            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

            _pedidoRepository.Atualizar(pedido);

            return RedirectToAction(nameof(Visualizar), new { id = id });
        }
    }
}