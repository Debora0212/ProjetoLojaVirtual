using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Email
{
    public class GerenciarEmail
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
     

        public GerenciarEmail(SmtpClient smtp, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _smtp = smtp;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void EnviarContatoPorEmail(Contato contato)
        {
            string corpoMsg = string.Format("<h2>Contato - Loja Virtual</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>Email: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                "<br />Email enviado automaticamente do site Loja Virtual.",
                contato.Nome,
                contato.Email,
                contato.Texto
                );

            /*
             * MailMessage -> construir a mensagem
             * */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add("deb.santos.0212@gmail.com");
            mensagem.Subject = "Contato - LojaVirtual - Email:" + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar mensagem via SMTP
            _smtp.Send(mensagem);
        }

        public void EnviarSenhaParaColaboradorPorEmail(Colaborador colaborador)
        {
            string corpoMsg = string.Format("<h2>Colaborador - Loja Virtual</h2>" +
                "Sua senha é:" +
                "<h3>{0}</h3>", colaborador.Senha);

            /*
             * MailMessage -> construir a mensagem
             * */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add(colaborador.Email);
            mensagem.Subject = "Colaborador - LojaVirtual - Senha do colaborador:" + colaborador.Nome;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar mensagem via SMTP
            _smtp.Send(mensagem);
        }

        public void EnviarDadosDoPedido(Cliente cliente, Pedido pedido)
        {
            string corpoMsg = string.Format("<h2>Pedido - LojaVirtual</h2>" +

                "Pedido realizado com sucesso!<br />" +
                "<h3>Nº {0}</h3>" +
                "<br /> Faça o login em nossa loja virtual e acompanhe o andamento.",
                pedido.Id + "-" + pedido.TransactionId

            );


            /*
             * MailMessage -> Construir a mensagem
             */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add(cliente.Email);
            mensagem.Subject = "LojaVirtual - Pedido - " + pedido.Id + "-" + pedido.TransactionId;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via SMTP
            _smtp.Send(mensagem);
        }

        public void EnviarLinkResetarSenha(dynamic usuario, string idCrip)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string url = "";
            if (usuario.GetType() == typeof(Cliente))
            {
                url = $"{request.Scheme}://{request.Host}/Cliente/Home/CriarSenha/{idCrip}";
            }
            else
            {
                url = $"{request.Scheme}://{request.Host}/Colaborador/Home/CriarSenha/{idCrip}";
            }


            string corpoMsg = string.Format(
                "<h2>Criar nova Senha para {1}({2})</h2>" +
                "Clique no link abaixo para criar uma nova senha!<br />" +
                "<a href='{0}' target='_blank'>{0}</a>",
                url,
                usuario.Nome,
                usuario.Email
            );
            
            /*
             * MailMessage -> Construir a mensagem
             */
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add(usuario.Email);
            mensagem.Subject = "LojaVirtual - Criar nova senha - " + usuario.Nome;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar Mensagem via SMTP
            _smtp.Send(mensagem);
        }
    }
}
