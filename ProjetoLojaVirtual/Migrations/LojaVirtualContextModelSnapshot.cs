﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjetoLojaVirtual.Database;

namespace ProjetoLojaVirtual.Migrations
{
    [DbContext(typeof(LojaVirtualContext))]
    partial class LojaVirtualContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProjetoLojaVirtual.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriaPaiId");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoriaPaiId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bairro")
                        .IsRequired();

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("CPF")
                        .IsRequired();

                    b.Property<string>("Cidade")
                        .IsRequired();

                    b.Property<string>("Complemento")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Endereco")
                        .IsRequired();

                    b.Property<string>("Estado")
                        .IsRequired();

                    b.Property<DateTime>("Nascimento");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Numero");

                    b.Property<string>("Senha")
                        .IsRequired();

                    b.Property<string>("Sexo")
                        .IsRequired();

                    b.Property<string>("Situacao");

                    b.Property<string>("Telefone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.Colaborador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Senha")
                        .IsRequired();

                    b.Property<string>("Tipo");

                    b.HasKey("Id");

                    b.ToTable("Colaboradores");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.EnderecoEntrega", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bairro")
                        .IsRequired();

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Cidade")
                        .IsRequired();

                    b.Property<int?>("ClienteId");

                    b.Property<string>("Complemento")
                        .IsRequired();

                    b.Property<string>("Endereco")
                        .IsRequired();

                    b.Property<string>("Estado")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Numero");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("EnderecosEntrega");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.Imagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Caminho");

                    b.Property<int>("ProdutoId");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("Imagens");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.NewsletterEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("NewsletterEmails");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.ProdutoAgregador.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Altura");

                    b.Property<int>("CategoriaId");

                    b.Property<int>("Comprimento");

                    b.Property<string>("Descricao")
                        .IsRequired();

                    b.Property<int>("Largura");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<double>("Peso");

                    b.Property<int>("Quantidade");

                    b.Property<decimal>("Valor");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.Categoria", b =>
                {
                    b.HasOne("ProjetoLojaVirtual.Models.Categoria", "CategoriaPai")
                        .WithMany()
                        .HasForeignKey("CategoriaPaiId");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.EnderecoEntrega", b =>
                {
                    b.HasOne("ProjetoLojaVirtual.Models.Cliente", "Cliente")
                        .WithMany("EnderecosEntrega")
                        .HasForeignKey("ClienteId");
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.Imagem", b =>
                {
                    b.HasOne("ProjetoLojaVirtual.Models.ProdutoAgregador.Produto", "Produto")
                        .WithMany("Imagens")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjetoLojaVirtual.Models.ProdutoAgregador.Produto", b =>
                {
                    b.HasOne("ProjetoLojaVirtual.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
