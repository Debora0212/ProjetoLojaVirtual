﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoLojaVirtual.Libraries.Json.Resolver;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers
{
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private LoginCliente _loginCliente;

        public PedidoController(IPedidoRepository pedidoRepository, LoginCliente loginCliente)
        {
            _pedidoRepository = pedidoRepository;
            _loginCliente = loginCliente;
        }
        public IActionResult Index(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (pedido.ClienteId != _loginCliente.GetCliente().Id)
            {
                //TODO - Implementar página HTML bonita para acesso negado/ERRO4XX/ERRO5XX
                return new ContentResult() { Content = "Acesso negado. Cliente não autorizada para este pedido." };
            }

            ViewBag.Produtos = JsonConvert.DeserializeObject<List<ProdutoItem>>(
                pedido.DadosProdutos,
                new JsonSerializerSettings() { ContractResolver = new ProdutoItemResolver<List<ProdutoItem>>() }
            );

            var transacao = JsonConvert.DeserializeObject<TransacaoPagarMe>(pedido.DadosTransaction);

            ViewBag.Transacao = transacao;

            return View(pedido);
        }
    }
}