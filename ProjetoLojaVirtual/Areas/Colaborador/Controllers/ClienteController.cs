using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Lang;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using ProjetoLojaVirtual.Repositories.Contracts;
using X.PagedList;

namespace ProjetoLojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public IActionResult Index(int? pagina, string pesquisa)
        {
            IPagedList<Cliente> clientes = _clienteRepository.ObterTodosClientes(pagina, pesquisa);
            return View(clientes);
        }

        [ValidateHttpReferer]
        public IActionResult AtivarDesativar(int id)
        {
             Cliente cliente = _clienteRepository.ObterCliente(id);
            cliente.Situacao = (cliente.Situacao == SituacaoConstant.Ativo) ? cliente.Situacao = SituacaoConstant.Desativado : cliente.Situacao = SituacaoConstant.Ativo;
            _clienteRepository.Atualizar(cliente);

            TempData["MSG_S"] = Mensagem.MSG_S001;

            return RedirectToAction(nameof(Index));
        }
    }
}