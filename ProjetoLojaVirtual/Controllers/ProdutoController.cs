using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Models;

namespace ProjetoLojaVirtual.Controllers
{
    public class ProdutoController : Controller
    {
        public ActionResult Visualizar()
        {
            Produto produto = GetProduto();

            return View(produto);
            //return new ContentResult() { Content = "<h3>Produto -> Visualizar<h3>", ContentType = "text/html" };
        }

        private Produto GetProduto()
        {
            return new Produto()
            {
                Id = 1,
                Nome = "Xbox One X",
                Descricao = "Jogue em 4k",
                Valor = 2000.00M
            };
        }
    }
}   