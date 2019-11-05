using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Email
{
    public class ContatoEmail
    {
        public static void EnviarContatoPorEmail(Contato contato)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("deb.santos.0212@gmail.com", "");
                smtp.EnableSsl = true;
            }

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
            mensagem.From = new MailAddress("deb.santos.0212@gmail.com");
            mensagem.To.Add("deb.santos.0212@gmail.com");
            mensagem.Subject = "Contato - LojaVirtual - Email:" + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar mensagem via SMTP
            smtp.Send(mensagem);
        }
    }
}
