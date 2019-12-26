using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Areas.Cliente.Controllers
{
    [ClienteAutorizacao]
    public class EnderecoEntregaController : Controller
    {
        private LoginCliente _loginCliente;
        private IEnderecoEntregaRepository _enderecoEntregaRepository;

        public EnderecoEntregaController(LoginCliente loginCliente, IEnderecoEntregaRepository enderecoEntregaRepository)
        {
            _loginCliente = loginCliente;
            _enderecoEntregaRepository = enderecoEntregaRepository;
        }

        public IActionResult Index()
        {
            var cliente = _loginCliente.GetCliente();
            ViewBag.Cliente = cliente;
            ViewBag.Enderecos = _enderecoEntregaRepository.ObterTodosEnderecoEntregaCliente(cliente.Id);
            return View();
        }
        //CRUD - Cadastro, Listagem-OK, Atualizacao, Remover
    }
}