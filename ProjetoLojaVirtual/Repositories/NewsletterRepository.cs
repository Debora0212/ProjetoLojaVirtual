using ProjetoLojaVirtual.Database;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Repositories
{
    public class NewsletterRepository : INewsletterRepository
    {
        private LojaVirtualContext _banco;
        public NewsletterRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }
        public void Cadastrar(NewsletterEmail newsletter)
        {
            _banco.NewsletterEmails.Add(newsletter);
            _banco.SaveChanges();
        }

        public IEnumerable<NewsletterEmail> ObterTodosNewsletter()
        {
           return _banco.NewsletterEmails.ToList();
        }

        public int QuantidadeTotalNewsletters()
        {
            return _banco.NewsletterEmails.Count();
        }
    }
}
