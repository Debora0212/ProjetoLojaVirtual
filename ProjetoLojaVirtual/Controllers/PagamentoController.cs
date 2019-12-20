using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        private GerenciarPagarMe _gerenciarPagarMe;

        public PagamentoController(
            GerenciarPagarMe gerenciarPagarMe,
            LoginCliente loginCliente,
            Cookie cookie,
            CookieCarrinhoCompra carrinhoCompra,
            IEnderecoEntregaRepository enderecoEntregaRepository,
            IProdutoRepository produtoRepository,
            IMapper mapper,
            WSCorreiosCalcularFrete wscorreios,
            CalcularPacote calcularPacote,
            CookieFrete cookieValorPrazoFrete)
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
            _cookie = cookie;
            _gerenciarPagarMe = gerenciarPagarMe;
        }
        [ClienteAutorizacao]
        [HttpGet]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                var enderecoEntrega = ObterEndereco();
                var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());

                int cep = int.Parse(Mascara.Remover(enderecoEntrega.CEP));

                ViewBag.Frete = ObterFrete(cep.ToString());
                List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
                ViewBag.Produtos = produtoItemCompleto;

                return View("Index");
            }
            TempData["MSG_E"] = Mensagem.MSG_E009;
            return RedirectToAction("EnderecoEntrega", "CarrinhoCompra");
        }

        [HttpPost]
        [ClienteAutorizacao]
        public IActionResult Index([FromForm]CartaoCredito cartaoCredito)
        {
            if (ModelState.IsValid)
            {
                //TODO - Integrar com Pagar.me, Salvar o pedido (Class), Redirecionar para a tela de pedido concluido;
                EnderecoEntrega enderecoEntrega = ObterEndereco();
                ValorPrazoFrete frete = ObterFrete(enderecoEntrega.CEP.ToString());
                List<ProdutoItem> produtos = CarregarProdutoDB();

                try
                {
                    dynamic pagarmeResposta = _gerenciarPagarMe.GerarPagCartaoCredito(cartaoCredito, enderecoEntrega, frete, produtos);

                    return new ContentResult() { Content = "Sucesso! " + pagarmeResposta.TransactionId };
                }
                catch (PagarMeException e)
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
                    TempData["MSG_E"] = sb.ToString();

                    return Index();
                }



            }
            else
            {
                return Index();
            }

        }


        private EnderecoEntrega ObterEndereco()
        {
            EnderecoEntrega enderecoEntrega = null;
            var enderecoEntregaId = int.Parse(_cookie.Consultar("Carrinho.Endereco", false).Replace("-end", ""));

            if (enderecoEntregaId == 0)
            {
                Cliente cliente = _loginCliente.GetCliente();
                enderecoEntrega = new EnderecoEntrega();
                enderecoEntrega.Nome = "Endereço do cliente";
                enderecoEntrega.Id = 0;
                enderecoEntrega.CEP = cliente.CEP;
                enderecoEntrega.Estado = cliente.Estado;
                enderecoEntrega.Cidade = cliente.Cidade;
                enderecoEntrega.Bairro = cliente.Bairro;
                enderecoEntrega.Endereco = cliente.Endereco;
                enderecoEntrega.Complemento = cliente.Complemento;
                enderecoEntrega.Numero = cliente.Numero;
            }
            else
            {
                var endereco = _enderecoEntregaRepository.ObterEnderecoEntrega(enderecoEntregaId);
                enderecoEntrega = endereco;
            }

            return enderecoEntrega;
        }
        private ValorPrazoFrete ObterFrete(string cepDestino)
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            var carrinhoHash = GerarHash(_cookieCarrinhoCompra.Consultar());
            int cep = int.Parse(Mascara.Remover(cepDestino));

            Frete frete = _cookieFrete.Consultar().Where(a => a.CEP == cep && a.CodCarrinho == carrinhoHash).FirstOrDefault();

            if (frete != null)
            {
                return frete.ListaValores.Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            }
            return null;
        }
    }
}