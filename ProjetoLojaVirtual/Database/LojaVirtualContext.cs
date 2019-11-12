using Microsoft.EntityFrameworkCore;
using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Database
{
    public class LojaVirtualContext : DbContext
    {
        public LojaVirtualContext(DbContextOptions<LojaVirtualContext> options) : base(options)
        {

        }

        public DbSet<Cliente>Clientes { get; set; }
        public DbSet<NewsletterEmail> NewsletterEmails { get; set; }
        public object NewsletterEmail { get; internal set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
    }
}
