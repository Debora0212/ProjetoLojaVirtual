using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Controllers
{
    public class ProdutoController : Controller
    {
        private ICategoriaRepository _categoriaRepository;
        private IProdutoRepository _produtoRepository;

        public ProdutoController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        [Route("/Produto/Categoria/{Slug}")]
        public IActionResult ListagemCategoria(string slug)
        {
            return View(_categoriaRepository.ObterCategoria(slug));
        }

        [HttpGet]
        public ActionResult Visualizar(int id)
        {
            return View(_produtoRepository.ObterProduto(id));
        }
    }
}