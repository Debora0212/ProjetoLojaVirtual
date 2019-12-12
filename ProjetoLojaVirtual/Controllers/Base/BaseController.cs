using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.CarrinhoCompra;
using ProjetoLojaVirtual.Libraries.Gerenciador.Frete;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers.Base
{
    public class BaseController : Controller
    {
        protected CookieCarrinhoCompra _cookieCarrinhoCompra;
        protected IProdutoRepository _produtoRepository;
        protected IMapper _mapper;
        protected WSCorreiosCalcularFrete _wscorreios;
        protected CalcularPacote _calcularPacote;
        protected CookieValorPrazoFrete _cookieValorPrazoFrete;

        public BaseController(CookieCarrinhoCompra cookieCarrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CalcularPacote calcularPacote, CookieValorPrazoFrete cookieValorPrazoFrete)
        {
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _wscorreios = wscorreios;
            _calcularPacote = calcularPacote;
            _cookieValorPrazoFrete = cookieValorPrazoFrete;
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
    }
}