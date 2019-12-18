﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Controllers.Base;
using ProjetoLojaVirtual.Libraries.CarrinhoCompra;
using ProjetoLojaVirtual.Libraries.Cookie;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Gerenciador.Frete;
using ProjetoLojaVirtual.Libraries.Lang;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Libraries.Texto;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        public PagamentoController(LoginCliente loginCliente, Cookie cookie, CookieCarrinhoCompra carrinhoCompra,IEnderecoEntregaRepository enderecoEntregaRepository,IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote, CookieFrete cookieValorPrazoFrete) : base(loginCliente, enderecoEntregaRepository, carrinhoCompra, produtoRepository, mapper, wscorreios, calcularPacote, cookieValorPrazoFrete)
        {
            _cookie = cookie;
        }

        [ClienteAutorizacao]
        [HttpGet]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                var enderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

                int cep = 0;
                if (enderecoEntregaId == 0)
                {
                    cep = int.Parse(Mascara.Remover(_loginCliente.GetCliente().CEP));
                }
                else
                {
                    var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
                    // enderecoEntrega = endereco;
                    cep = int.Parse(Mascara.Remover(endereco.CEP));

                }
                var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());

                Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();

                if (frete != null)
                {
                    ViewBag.Frete = frete.ListaValores.Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
                    List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                    ViewBag.Produtos = produtoItemCompleto;

                    return View("Index");
                }
                else
                {
                    return RedirectToAction("EnderecoEntrega", "CarrinhoCompra");
                }
            }
            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("EnderecoEntrega", "CarrinhoCompra");
        }

        [HttpPost]
        [ClienteAutorizacao]
        public IActionResult Index([FromForm] CartaoCredito cartaoCredito)
        {
            if (ModelState.IsValid)
            {
                //TODO- Integrar com Pagar.Me, salvar o pedido (Class), redirecionar p a tela  de pedido concluido.
                return View();
            }
            else
            {
                return Index();
            }
        }
    }
}
