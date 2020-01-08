﻿using Microsoft.EntityFrameworkCore;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Database
{
    public class LojaVirtualContext : DbContext
    {
        /*
         * EF-Core - ORM
         * ORM -> Biblioteca mapear Objetos para Banco de Dados Relacionais.
         */

        public LojaVirtualContext(DbContextOptions<LojaVirtualContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Cliente>Clientes { get; set; }
        public DbSet<NewsletterEmail> NewsletterEmails { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EnderecoEntrega> EnderecosEntrega { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoSituacao> PedidoSituacoes { get; set; }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
    }
}
