﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Repositories.Contracts;
using X.PagedList;

namespace ProjetoLojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    //TODO-Habilitar verificacao de login
    //[ColaboradorAutorizacao]
    public class CategoriaController : Controller
    {
        private ICategoriaRepository _categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult Index(int? pagina)
        {
            var categorias = _categoriaRepository.ObterTodasCategorias(pagina);
            return View(categorias);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Categoria categoria)
        {
            //TODO-Implementar
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Categoria categoria)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int Id)
        {
            return View();
        }

    }
}