﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoLojaVirtual.Libraries.Email;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Lang;
using ProjetoLojaVirtual.Libraries.Login;
using ProjetoLojaVirtual.Libraries.Seguranca;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private IEnderecoEntregaRepository _repositoryEnderecoEntrega;
        private IClienteRepository _repositoryCliente;
        private LoginCliente _loginCliente;
        private GerenciarEmail _gerenciarEmail;

        public HomeController(IEnderecoEntregaRepository repositoryEnderecoEntrega, IClienteRepository repositoryCliente, LoginCliente loginCliente, GerenciarEmail gerenciarEmail)
        {
            _repositoryEnderecoEntrega = repositoryEnderecoEntrega;
            _repositoryCliente = repositoryCliente;
            _loginCliente = loginCliente;
            _gerenciarEmail = gerenciarEmail;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Models.Cliente cliente, string returnUrl = null)
        {
            Models.Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                _loginCliente.Login(clienteDB);

                if (returnUrl == null)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Recuperar([FromForm]Models.Cliente cliente)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Nascimento");
            ModelState.Remove("Sexo");
            ModelState.Remove("CPF");
            ModelState.Remove("Telefone");
            ModelState.Remove("CEP");
            ModelState.Remove("Estado");
            ModelState.Remove("Cidade");
            ModelState.Remove("Bairro");
            ModelState.Remove("Endereco");
            ModelState.Remove("Complemento");
            ModelState.Remove("Numero");
            ModelState.Remove("Senha");
            ModelState.Remove("ConfirmacaoSenha");

            if (ModelState.IsValid)
            {
                var clienteDoBancoDados = _repositoryCliente.ObterClientePorEmail(cliente.Email);

                if (clienteDoBancoDados != null)
                {
                     string idCrip = Base64Cipher.Base64Encode(clienteDoBancoDados.Id.ToString());
                    _gerenciarEmail.EnviarLinkResetarSenha(clienteDoBancoDados, idCrip);

                    TempData["MSG_S"] = Mensagem.MSG_S004;

                    ModelState.Clear();
                }
                else
                {
                    TempData["MSG_E"] = Mensagem.MSG_E014;
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult CriarSenha(string id)
        {
            try
            {
                var idClienteDecrip = Base64Cipher.Base64Decode(id);
                int idCliente;
                if ( !int.TryParse(idClienteDecrip, out idCliente))
                {
                    TempData["MSG_E"] = Mensagem.MSG_E015;
                }
            }
            catch (System.FormatException e)
            {
                TempData["MSG_E"] = Mensagem.MSG_E015;
            }
           
            return View();
        }

        [HttpPost]
        public IActionResult CriarSenha([FromForm]Models.Cliente cliente, string id)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Nascimento");
            ModelState.Remove("Sexo");
            ModelState.Remove("CPF");
            ModelState.Remove("Email");
            ModelState.Remove("Telefone");
            ModelState.Remove("CEP");
            ModelState.Remove("Estado");
            ModelState.Remove("Cidade");
            ModelState.Remove("Bairro");
            ModelState.Remove("Endereco");
            ModelState.Remove("Complemento");
            ModelState.Remove("Numero");

            if (ModelState.IsValid)
            {
                int idCliente;
                try
                {
                    var idClienteDecrip = Base64Cipher.Base64Decode(id);

                    if (!int.TryParse(idClienteDecrip, out idCliente))
                    {
                        TempData["MSG_E"] = Mensagem.MSG_E015;
                        return View();
                    }

                }
                catch (System.FormatException e)
                {
                    TempData["MSG_E"] = Mensagem.MSG_E015;
                    return View();
                }


                var clienteDB = _repositoryCliente.ObterCliente(idCliente);
                if (clienteDB != null)
                {
                    clienteDB.Senha = cliente.Senha;

                    _repositoryCliente.Atualizar(clienteDB);
                    TempData["MSG_S"] = Mensagem.MSG_S005;
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult Sair()
        {
            _loginCliente.Logout();

            return RedirectToAction("Index", "Home", new { area = "" });
        }
        
    }
}