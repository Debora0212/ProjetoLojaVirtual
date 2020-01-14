using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private IColaboradorRepository _repositoryColaborador;
        private LoginColaborador _loginColaborador;

        private IClienteRepository _clienteRepository;
        private IProdutoRepository _produtoRepository;
        private INewsletterRepository _newsletterRepository;
        private IPedidoRepository _pedidoRepository;

        public HomeController(IClienteRepository clienteRepository, IProdutoRepository produtoRepository,
            INewsletterRepository newsletterRepository, IPedidoRepository pedidoRepository, 
            IColaboradorRepository repositoryColaborador, LoginColaborador loginColaborador)
        {
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _newsletterRepository = newsletterRepository;
            _pedidoRepository = pedidoRepository;
            _repositoryColaborador = repositoryColaborador;
            _loginColaborador = loginColaborador;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Models.Colaborador colaborador)
        {
            Models.Colaborador colaboradorDB = _repositoryColaborador.Login(colaborador.Email, colaborador.Senha);

            if (colaboradorDB != null)
            {
                _loginColaborador.Login(colaboradorDB);

                return new RedirectResult(Url.Action(nameof(Painel)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }

        [ColaboradorAutorizacao]
        [ValidateHttpReferer]
        public IActionResult Logout()
        {
            _loginColaborador.Logout();
            return RedirectToAction("Login", "Home");
        }



        public IActionResult RecuperarSenha()
        {
            return View();
        }

        public IActionResult CadastrarNovaSenha()
        {
            return View();
        }



        [ColaboradorAutorizacao]
        public IActionResult Painel()
        {
            ViewBag.Clientes = _clienteRepository.QuantidadeTotalClientes();
            ViewBag.Newsletter = _newsletterRepository.QuantidadeTotalNewsletters();
            ViewBag.Produto = _produtoRepository.QuantidadeTotalProdutos();
            ViewBag.NumeroPedidos = _pedidoRepository.QuantidadeTotalPedidos();
            ViewBag.ValorTotalPedidos = _pedidoRepository.ValorTotalPedidos();
            ViewBag.QuantidadeBoletoBancario = _pedidoRepository.QuantidadeTotalBoletoBancario();
            ViewBag.QuantidadeCartaoCredito = _pedidoRepository.QuantidadeTotalCartaoCredito();

            return View();
        }

        public IActionResult GerarCSVNewsletter()
        {
            var news = _newsletterRepository.ObterTodosNewsletter();

            StringBuilder sb = new StringBuilder();

            foreach(var email in news)
            {
                sb.AppendLine(email.Email);
            }

            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
            return File(buffer, "text/csv", $"newsletter.csv");
        }
    }
}
