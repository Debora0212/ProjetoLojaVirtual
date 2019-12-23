﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagarMe;
using ProjetoLojaVirtual.Controllers.Base;
using ProjetoLojaVirtual.Libraries.CarrinhoCompra;
using ProjetoLojaVirtual.Libraries.Cookie;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Gerenciador.Frete;
using ProjetoLojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using ProjetoLojaVirtual.Libraries.Lang;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Libraries.Texto;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using ProjetoLojaVirtual.Models.ViewModels.Pagamento;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers
{
    [ClienteAutorizacao]
    [ValidateCookiePagamentoController]
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        private GerenciarPagarMe _gerenciarPagarMe;
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;

        public PagamentoController(
            GerenciarPagarMe gerenciarPagarMe,
            LoginCliente loginCliente,
            Cookie cookie,
            CookieCarrinhoCompra carrinhoCompra,
            IPedidoRepository pedidoRepository,
            IPedidoSituacaoRepository pedidoSituacaoRepository,
            IEnderecoEntregaRepository enderecoEntregaRepository,           
            IProdutoRepository produtoRepository,          
            IMapper mapper,
            WSCorreiosCalcularFrete wscorreios,
            CalcularPacote calcularPacote,
            CookieFrete cookieValorPrazoFrete          
            )
            : base(
                  loginCliente,
                  enderecoEntregaRepository,
                  carrinhoCompra,
                  produtoRepository,
                  mapper,
                  wscorreios,
                  calcularPacote,
                  cookieValorPrazoFrete)
        {       
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _cookie = cookie;     
            _gerenciarPagarMe = gerenciarPagarMe;          
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
            ValorPrazoFrete frete = ObterFrete();

            ViewBag.Frete = frete;
            ViewBag.Produtos = produtoItemCompleto;
            ViewBag.Parcelamentos = CalcularParcelamento(produtoItemCompleto);

            return View("Index");
        }

        [HttpPost]
        public IActionResult Index([FromForm]IndexViewModel indexViewModel)
        {
            if (ModelState.IsValid)
            {
                //TODO - Salvar o pedido (Class), Redirecionar para a tela de pedido concluido;
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete();
                List<ProdutoItem> produtos = CarregarProdutoDB();
                Parcelamento parcela = BuscarParcelamento(produtos, indexViewModel.Parcelamento.Numero);

                try
                {
                    Transaction transaction = _gerenciarPagarMe.GerarPagCartaoCredito(indexViewModel.CartaoCredito, parcela, enderecoEntrega, frete, produtos);

                    Pedido pedido = new Pedido();
                    pedido.ClienteId = int.Parse(transaction.Customer.Id);
                    pedido.TransactionId = transaction.Id;
                    pedido.FreteEmpresa = "ECT-Correios";
                    pedido.FormPagamento = (transaction.PaymentMethod == 0) ? "Cartão de Credito" : "Boleto";
                    pedido.ValorTotal = ObterValorTotalCompra(produtos);
                    pedido.DadosTransaction = transaction; // Serializar;
                    pedido.DadosProdutos = produtos;// Serializar;
                    pedido.DataRegistro = DateTime.Now;
                    pedido.Situacao = "";//TODO -Situacao - criar constants.

                    _pedidoRepository.Cadastrar(pedido);
                    PedidoSituacao pedidoSituacao = new PedidoSituacao();
                    pedidoSituacao.PedidoId = pedido.Id;
                    pedidoSituacao.Data = DateTime.Now;
                    pedidoSituacao.Dados = new { Transaction = transaction, Produtos = produtos };//JSON
                    pedidoSituacao.Situacao = "";

                    _pedidoRepository.Cadastrar(pedidoSituacao);

                    return new ContentResult() { Content = "Sucesso! Cartão de Crédito" + transaction.Id };
                }
                catch (PagarMeException e)
                {
                    TempData["MSG_E"] = MontarMensagensDeErro(e);

                    return Index();
                }
            }
            else
            {
                return Index();
            }

        }

        public IActionResult BoletoBancario()
        {
            EnderecoEntrega enderecoEntrega = ObterEndereco();
            ValorPrazoFrete frete = ObterFrete();
            List<ProdutoItem> produtos = CarregarProdutoDB();
            var valorTotal = ObterValorTotalCompra(produtos);

            try
            {
                Transaction transaction = _gerenciarPagarMe.GerarBoleto(valorTotal);

                //TODO - Redirecionar para Página de Sucesso!
                return new ContentResult() { Content = "Sucesso! Boleto - " + transaction.Id };

            }
            catch (PagarMeException e)
            {
                TempData["MSG_E"] = MontarMensagensDeErro(e);
                return RedirectToAction(nameof(Index));
            }
        }








        private Parcelamento BuscarParcelamento(List<ProdutoItem> produtos, int numero)
        {
            return _gerenciarPagarMe.CalcularPagamentoParcelado(ObterValorTotalCompra(produtos)).Where(a => a.Numero == numero).First();
        }
        private EnderecoEntrega ObterEndereco()
        {
            EnderecoEntrega enderecoEntrega = null;
            var enderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

            if (enderecoEntregaId == 0)
            {
                Cliente cliente = _loginCliente.GetCliente();
                enderecoEntrega = _mapper.Map<EnderecoEntrega>(cliente);
            }
            else
            {
                var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
                enderecoEntrega = endereco;
            }

            return enderecoEntrega;
        }
        private ValorPrazoFrete ObterFrete()
        {
            var enderecoEntrega = ObterEndereco();
            int cep = int.Parse(Mascara.Remover(enderecoEntrega.CEP));
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());

            Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();

            if (frete != null)
            {
                return frete.ListaValores.Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            }
            return null;
        }
        private decimal ObterValorTotalCompra(List<ProdutoItem> produtos)
        {
            ValorPrazoFrete frete = ObterFrete();
            decimal total = Convert.ToDecimal(frete.Valor);

            foreach (var produto in produtos)
            {
                total += produto.Valor;
            }

            return total;
        }
        private List<SelectListItem> CalcularParcelamento(List<ProdutoItem> produtos)
        {
            var total = ObterValorTotalCompra(produtos);
            var parcelamento = _gerenciarPagarMe.CalcularPagamentoParcelado(total);


            return parcelamento.Select(a => new SelectListItem(
                String.Format(
                    "{0}x {1} {2} - TOTAL: {3}",
                    a.Numero, a.ValorPorParcela.ToString("C"), (a.Juros) ? "c/ juros" : "s/ juros", a.Valor.ToString("C")
                ),
                a.Numero.ToString()
            )).ToList();
        }
        private string MontarMensagensDeErro(PagarMeException e)
        {
            StringBuilder sb = new StringBuilder();

            if (e.Error.Errors.Count() > 0)
            {
                sb.Append("Erro no pagamento: ");
                foreach (var erro in e.Error.Errors)
                {
                    sb.Append("- " + erro.Message + "<br />");
                }
            }
            return sb.ToString();
        }
    }
}
