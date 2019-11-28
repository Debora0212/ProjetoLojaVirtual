using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Component
{
    public class MenuViewComponent : ViewComponent
    {
        private ICategoriaRepository _categoriaRepository;
        public MenuViewComponent(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        //Logica do componente, segue uma numeclatura padrao (public async Task<IViewComponentResult>InvokeAsync)
        public async Task<IViewComponentResult> InvokeAsync()
        {
           var ListaCategoria = _categoriaRepository.ObterTodasCategorias().ToList();
            return View(ListaCategoria);
        }
    }
}
