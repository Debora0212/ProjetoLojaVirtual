using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoLojaVirtual.Libraries.CarrinhoCompra;
using ProjetoLojaVirtual.Libraries.Gerenciador.Frete;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Libraries.Seguranca;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers.Base
{
    public class BaseController : Controller
    {
        protected LoginCliente _loginCliente;
        protected IEnderecoEntregaRepository _enderecoEntregaRepository;
        protected CookieCarrinhoCompra _cookieCarrinhoCompra;
        protected IProdutoRepository _produtoRepository;
        protected IMapper _mapper;
        protected WSCorreiosCalcularFrete _wscorreios;
        protected CalcularPacote _calcularPacote;
        protected CookieFrete _cookieFrete;

        public BaseController(LoginCliente loginCliente, IEnderecoEntregaRepository enderecoEntregaRepository, CookieCarrinhoCompra cookieCarrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote, CookieFrete cookieFrete)
        {
            _loginCliente = loginCliente;
            _enderecoEntregaRepository = enderecoEntregaRepository;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _wscorreios = wscorreios;
            _calcularPacote = calcularPacote;
            _cookieFrete = cookieFrete;
        }
        protected List<ProdutoItem> CarregarProdutoDB()
        {
            List<ProdutoItem> produtoItemNoCarrinho = _cookieCarrinhoCompra.Consultar();

            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemNoCarrinho)
            {
                Produto produto = _produtoRepository.ObterProduto(item.Id);

                ProdutoItem produtoItem = _mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);
            }

            return produtoItemCompleto;
        }

        protected string GerarHash(object obj)
        {
            return StringMD5.MD5Hash(JsonConvert.SerializeObject(obj));
        }
    }
}